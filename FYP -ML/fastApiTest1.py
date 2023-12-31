from fastapi import FastAPI
from pydantic import BaseModel
#from pyFunction1 import NewsClassifier
from NaiveBayes import NewsClassifier


class News(BaseModel):
    Topic: str
    Content: str 

app = FastAPI()
obj=NewsClassifier()
obj.trainData()
# print(News)
@app.post("/news/")
async def classify_news(classification: News):
     reslt = obj.predict(classification.Content);
     return {
          "result":reslt
     }



