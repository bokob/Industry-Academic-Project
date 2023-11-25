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

import random

# Firebase database 인증 및 앱 초기화
# Fetch the service account key JSON file contents
cred = credentials.Certificate('myKey.json')
# Initialize the app with a service account, granting admin privileges
firebase_admin.initialize_app(cred, {
    'databaseURL': 'https://udangtangtang-2a355-default-rtdb.firebaseio.com/',
    'storageBucket' : 'udangtangtang-2a355.appspot.com' # 앞에 'gs://' , 뒤에 '/' 붙이면 오류남
})

ref = db.reference() # RealTimeDataBase 위치 지정
bucket = storage.bucket() # Storage

app = FastAPI()

@app.get("/test")
async def root():

    ref.update({"엄준식":"성공"})
    return {"message": "Hello World"}

### 퀴즈 관련 함수 ###
@app.get("/createMakeQuizList")
async def createMakeQuizList(): # 퀴즈 스키마 만들기

    d={
        "quiz":{
            "뉴트리아" : [],
            "늑대거북" : [],
            "악어거북" : [],
            "플로리다붉은배거북" : [],
            "리버쿠터" : [],
            "중국줄무늬목거북" : [],
            "붉은귀거북" : [],
            "황소개구리" : [],
            "배스" : [],
            "브라운송어" : [],
            "블루길" : [],
            "갈색날개매미충" : [],
            "긴다리비틀개미" : [],
            "꽃매미" : [],
            "등검은말벌" : [],
            "미국선녀벌레" : [],
            "붉은불개미" : [],
            "빗살무늬미주메뚜기" : [],
            "아르헨티나개미" : [],
            "열대불개미" : [],
            "미국가재" : []
        }
    }

    for key in d["quiz"].keys(): # null 값이 처음부터 들어갈 수 없어서 기초 동물별로 기초 질문 하나씩 넣기
        s = "이 동물의 이름은\n\n" + key +"(이)가 맞는가?"
        d["quiz"][key].append({
            "animal" : key,
            "problem" : s,
            "answer" : True,
            "solution" : "생긴 그대로입니다."
            })

    ref.update(d)

    return d

class Quiz(BaseModel):
    animal: str
    img:str
    problem: str
    answer: bool
    solution: str

# 퀴즈 추가
@app.post("/addQuiz")
async def addQuiz(data : Quiz):
    animal = data.animal
    newQuiz = ref.get()

    quizData = {
        "animal": animal,
        "problem": data.problem,
        "answer":data.answer,
        "solution":data.solution
     }
    
    newQuiz['quiz'][animal].append(quizData)

    ref.update(newQuiz)

    return newQuiz

# 퀴즈 반환
@app.get("/quiz")
async def getQuiz():

    tmp = ref.get()
    keyList = list(tmp['quiz'].keys())
    quizList=[]

    while len(quizList)!=20:
        randomKey = random.choice(keyList)
        randomQuiz = random.choice(tmp['quiz'][randomKey])
        if randomQuiz not in quizList: quizList.append(randomQuiz)

    # 1. 파이어 베이스에서 퀴즈들을 가져온다.
    # 2. 퀴즈 중에서 키값들을 리스트로 만들어서 keyList 변수를 만든다.
    # 3. 랜덤으로 20개의 퀴즈를 가져온다.(중복 체크 해야함)
    # 4. 유니티로 넘겨준다.
    
    r = {"quizList" : quizList}

    return r

@app.get("/image") # 파이어 베이스에서 이미지 불러오는 코드 (퀴즈용)
async def getImage():
    blob = bucket.blob('um.png') # 엄준식 사진 가져오기
    image_data = blob.download_as_bytes()

    return StreamingResponse(io.BytesIO(image_data), media_type="image/png")

### 그래프 그리는 부분 ###
class MyData(BaseModel):
    date: List[str]
    answer: List[int]

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
async def create_graph(data : MyData):
    # Access JSON data sent from Unity
    date = data.date
    answer = data.answer
    data_dict = dict(zip(date,answer))

    # Generate the graph
    image_data = generate_graph(data_dict)

    return StreamingResponse(image_data, media_type="image/png")