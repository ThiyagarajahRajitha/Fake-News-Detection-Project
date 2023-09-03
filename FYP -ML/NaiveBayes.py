import pandas as pandas
import numpy as np
import pickle
from regex import Pattern
from sklearn.model_selection import KFold
from sklearn.naive_bayes import MultinomialNB
from sklearn import metrics
from sklearn.pipeline import Pipeline
from matplotlib import pyplot as plt
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score, confusion_matrix
from sklearn.model_selection import cross_val_score
import string
import re
import nltk
from nltk.corpus import stopwords
import itertools as itertools

class NewsClassifier:
    global nb_pipeline
    global tfidf_vectorizer
    tfidf_vectorizer = TfidfVectorizer(stop_words = 'english', max_df = 0.7)
    nb_pipeline = Pipeline([('NBTV',tfidf_vectorizer),('nb_clf', MultinomialNB())])#builing naive classifier model
    # global target_word
    # target_word= 'covid'
    # global pattern 
    # pattern = re.compile(r'\b{}\b'.format(re.escape(target_word)), re.IGNORECASE)

    def __init__(self):
        pass
    def trainData(self):
        df=pandas.read_csv('Final13022.csv',encoding='windows-1252')
        y=df.label_text
        X_train, X_test,y_train, y_test=train_test_split(df['text'],y,test_size=0.20,random_state=9)
		
        cleaned_corpus =[]
        for tt in X_train:
            cleaned_text =re.sub(r'[^\w\s]', '', tt)#remove punctuation
            cleaned_text=re.sub('\n','',cleaned_text) #Remove New lines
            cleaned_text = cleaned_text.lower()
            cleaned_text = self.remove_stopwords(cleaned_text)
            cleaned_corpus.append(cleaned_text)
        
        tokenized_corpus = []   
        for sentence in cleaned_corpus:
            tokens = nltk.word_tokenize(sentence)
            tokenized_corpus.append(tokens)

        joined_tokens = [' '.join(tokens) for tokens in tokenized_corpus]

        tfidf_vectorizer = TfidfVectorizer(stop_words = 'english', max_df = 0.7)
        #tfidf_train = tfidf_vectorizer.fit_transform(joined_tokens)
        nb_pipeline.fit(joined_tokens, y_train)

    def predict(self, news):
        testData = [news]
        cleaned_corpus_test =[]
        for tt in testData:
            cleaned_text =re.sub(r'[^\w\s]', '', tt)#remove punctuation
            cleaned_text=re.sub('\n','',cleaned_text) #Remove New lines
            cleaned_text = cleaned_text.lower()
            cleaned_text = self.remove_stopwords(cleaned_text)
            cleaned_corpus_test.append(cleaned_text)
    
        tokenized_corpus_test = []   
        for sentence in cleaned_corpus_test:
            tokens = nltk.word_tokenize(sentence)
            tokenized_corpus_test.append(tokens)

        joined_tokens_test = [' '.join(tokens) for tokens in tokenized_corpus_test]
    
        #tfidf_test = tfidf_vectorizer.transform(joined_tokens_test)
        
        predicted_nbt = nb_pipeline.predict(joined_tokens_test)
        if predicted_nbt[0] =='Real':
            reslt = "Real"
        elif predicted_nbt[0] == 'Fake':
            reslt = "Fake"
        else:
            reslt = "Not Sure"

        print(reslt)
        return reslt
               
            
    def remove_stopwords(self,text):
        stop_words = set(stopwords.words('english'))
        tokens = nltk.word_tokenize(text)
        filtered_tokens = [word for word in tokens if word.lower() not in stop_words]
        filtered_text = ' '.join(filtered_tokens)
        return filtered_text




