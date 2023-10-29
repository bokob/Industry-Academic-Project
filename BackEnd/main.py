from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from fastapi.responses import StreamingResponse

import firebase_admin
from firebase_admin import credentials
from firebase_admin import db
from firebase_admin import storage

from PIL import Image
import io
import matplotlib.pyplot as plt

from typing import List, Dict


# Firebase database 인증 및 앱 초기화
# Fetch the service account key JSON file contents
cred = credentials.Certificate('myKey.json')
# Initialize the app with a service account, granting admin privileges
firebase_admin.initialize_app(cred, {
    'databaseURL': 'https://udangtangtang-2a355-default-rtdb.firebaseio.com/',
    'storageBucket' : 'udangtangtang-2a355.appspot.com' # 앞에 'gs://' , 뒤에 '/' 붙이면 오류남
})

ref = db.reference() # db 위치 지정
bucket = storage.bucket()

app = FastAPI()


@app.get("/test")
async def root():

    ref.update({"엄준식":"성공"})
    return {"message": "Hello World"}


@app.get("/image") # 파이어 베이스에서 이미지 불러오는 코드 (퀴즈용)
async def getImage():
    blob = bucket.blob('um.png')
    image_data = blob.download_as_bytes()

    return StreamingResponse(io.BytesIO(image_data), media_type="image/png")


### 그래프 그리는 부분 ###
class MyData(BaseModel):
    date: List[str]
    count: List[int]


def generate_graph(data): # 그래프 그리기
    # 그래프 생성
    x = list(data.keys())
    y = list(data.values())

    # 그래프 크기 설정
    plt.figure(figsize=(10, 6))

    plt.title("Statistics for the last seven days", fontsize=20)
    plt.bar(x, y)
    
    plt.xlabel('Date', fontsize=12)
    plt.ylabel('Count', fontsize=12, labelpad=5, rotation=0, loc='top')

    plt.xticks(fontsize=14)
    plt.yticks(range(0,20 + 1,2))

    
    # 그래프를 이미지로 렌더링한 후 이진 데이터로 변환
    image_data = io.BytesIO()
    plt.savefig(image_data, format="png")
    plt.close()
    image_data.seek(0)
    
    return image_data

@app.post('/graph')
async def create_graph(data: MyData):
    # Access JSON data sent from Unity
    date = data.date
    count = data.count
    data_dict = dict(zip(date,count))

    # Generate the graph
    image_data = generate_graph(data_dict)

    return StreamingResponse(image_data, media_type="image/png")