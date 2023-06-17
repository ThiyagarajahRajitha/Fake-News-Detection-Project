#!/usr/bin/env python
# coding: utf-8

# In[1]:

import numpy as np
import pandas as pd


# In[2]:


from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score, confusion_matrix
from sklearn.model_selection import cross_val_score


# In[3]:


df=pd.read_csv('Final1302.csv',encoding='windows-1252')#had some decode error. so used encoding
df


# In[4]:


df=pd.read_csv('Final1302.csv',encoding='windows-1252')
conversion_dict={0:'False',1:'True'}
df['label']=df['label'].replace(conversion_dict)#Replace 1,0 to real and fake
df


# In[5]:


#Check the balance of the data
df.label.value_counts()#Not balanced. Have to find a way to solve this


# In[6]:


import string
import re
def word_drop(text):
    text=text.lower()
    text=re.sub('\[.*?\]','',text)
    text=re.sub("\\W","",text)
    text=re.sub('https?://\S+|www\.\S+','',text)
    text=re.sub('<.*?>+','',text)
    text=re.sub('[%s]' % re.escape(string.punctuation),'',text)
    text=re.sub('\n','',text)
    text=re.sub('\w"\d\w"','',text)
    return text


# In[7]:


df["text"]=df["text"].apply(word_drop)


# In[8]:


df.head(5)


# In[9]:


X_train, X_test,y_train, y_test=train_test_split(df['text'],df['label'],test_size=0.25,random_state=9,shuffle=True)


# In[10]:


tfidf_vectorizer=TfidfVectorizer(stop_words='english', max_df=0.75)


# In[11]:


vec_train=tfidf_vectorizer.fit_transform(X_train.values.astype('U'))
vec_test=tfidf_vectorizer.transform(X_test.values.astype('U'))


# In[12]:


from sklearn.neighbors import KNeighborsClassifier
knn=KNeighborsClassifier(n_neighbors=2)
knn.fit(vec_train,y_train)


# In[13]:


y_pred=knn.predict(vec_test)
score=accuracy_score(y_test,y_pred)
print(f'PAC Accuracy: {round(score*100,2)}%')


# In[14]:


#Or
knn.score(vec_test,y_test)


# In[84]:


#Can use gridSearchCV or Kfold crossvalidation to change the k value and get the optimal value


# In[15]:


cm=confusion_matrix(y_test,y_pred, labels=['True','False'])
cm



# In[18]:


import matplotlib.pyplot as plt
import seaborn as sn
plt.figure(figsize=(5,3))
sn.heatmap(cm,annot=True)
plt.xlabel('Predicted')
plt.ylabel('Actual')


# In[19]:


from sklearn.metrics import classification_report
print(classification_report(y_test,y_pred))


# In[20]:


df_true=pd.read_csv('Real.csv',encoding='windows-1252')
df_fake=pd.read_csv('Fake.csv',encoding='windows-1252')
df_final=pd.concat([df_true,df_fake])


# In[89]:


def find_label(newtext):
    vec_newtext=tfidf_vectorizer.transform([newtext])
    y_pred=knn.predict(vec_newtext)
    return y_pred[0]


# In[90]:


find_label((df_true['text'][0]))#predicting true news as false


# In[91]:


find_label((df_fake['text'][0]))


# In[92]:


sum([1 if find_label((df_true['text'][i]))=='True' else 0 for i in range(len(df_true['text']))])/df_true['text'].size


# In[94]:


sum([1 if find_label((df_fake['text'][i]))=='false' else 0 for i in range(len(df_fake['text']))])/df_fake['text'].size


# In[42]:


#Only 24% times the model predicts the actual results as true for the unknown dataset which all the news are true
#Only 21% times the model predicts the actual results as true for the unknown dataset which all the news are false


# In[ ]:




