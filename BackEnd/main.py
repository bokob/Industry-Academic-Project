from fastapi import FastAPI
from fastapi.responses import StreamingResponse

import firebase_admin
from firebase_admin import credentials
from firebase_admin import db
from firebase_admin import storage

from PIL import Image
import base64
import io
import matplotlib.pyplot as plt

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


@app.get("/image")
async def getImage():
    blob = bucket.blob('um.png')
    image_data = blob.download_as_bytes()

    return StreamingResponse(io.BytesIO(image_data), media_type="image/png")

def generate_graph():
    # 그래프 생성
    plt.plot([1, 2, 3, 4])
    plt.ylabel('some numbers')
    
    # 그래프를 이미지로 렌더링한 후 이진 데이터로 변환
    image_data = io.BytesIO()
    plt.savefig(image_data, format="png")
    plt.close()
    image_data.seek(0)
    
    return image_data

@app.get('/graph')
async def getGraph():
    # 그래프 이미지를 반환
    image_data = generate_graph()
    return StreamingResponse(image_data, media_type="image/png")