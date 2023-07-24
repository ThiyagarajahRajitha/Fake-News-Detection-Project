import pandas as pandas
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score, confusion_matrix
from sklearn.model_selection import cross_val_score
import string
import re
import nltk
from nltk.corpus import stopwords
nltk.download('stopwords')
nltk.download('punkt')
class NewsClassifier:
	global knn
	global tfidf_vectorizer
	tfidf_vectorizer=TfidfVectorizer()
	
	def __init__(self):
		pass
	def trainData(self):
		df=pandas.read_csv('Final13022.csv',encoding='windows-1252')
		conversion_dict={0:'False',1:'True'}
		df['label']=df['label'].replace(conversion_dict)#Replace 1,0 to real and fake
		
		corpus = df['text'].tolist()
		labels = df['label'].tolist()

		cleaned_corpus =[]
		for tt in corpus:
			cleaned_text =re.sub(r'[^\w\s]', '', tt)#remove punctuation
			cleaned_text=re.sub('\n','',cleaned_text) #Remove New lines
			cleaned_text = cleaned_text.lower()
			cleaned_text = self.remove_stopwords(cleaned_text)
			cleaned_corpus.append(cleaned_text)

		# Tokenize the corpus
		tokenized_corpus = []   
		for sentence in cleaned_corpus:
			tokens = nltk.word_tokenize(sentence)
			tokenized_corpus.append(tokens) 

		# Convert tokens to features using TF-IDF vectorization
		X = tfidf_vectorizer.fit_transform([' '.join(tokens) for tokens in tokenized_corpus])
		X_train, X_test,y_train, y_test=train_test_split(X,labels,test_size=0.20,random_state=9,shuffle=True)
		from sklearn.neighbors import KNeighborsClassifier
		
		global knn
		knn=KNeighborsClassifier(n_neighbors=2)
		knn.fit(X_train,y_train)
		


	def predict(self, news):
		testData = [news]
		cleaned_text = []

		for tt in testData:
			cleaned =re.sub(r'[^\w\s]', '', tt)#remove punctuation
			cleaned=re.sub('\n','',cleaned) #Remove New lines
			cleaned = cleaned.lower()
			cleaned = self.remove_stopwords(cleaned)
			cleaned_text.append(cleaned)

		tokenized_text = []   
		for sentence in cleaned_text:
			tokens = nltk.word_tokenize(sentence)
			tokenized_text.append(tokens)
		newTest_X = tfidf_vectorizer.transform([' '.join(tokens) for tokens in tokenized_text])
		y_pred=knn.predict(newTest_X)
		if y_pred[0] == 'False':
			reslt = "Fake"
		elif y_pred[0] =='Real':
			reslt = "Real"
		else:
			reslt = "Not Sure"

		print(reslt)
		return reslt
		
		#print(y_pred[0])
		
		#return y_pred[0]
	
	def remove_stopwords(self,text):
		stop_words = set(stopwords.words('english'))
		tokens = nltk.word_tokenize(text)
		filtered_tokens = [word for word in tokens if word.lower() not in stop_words]
		filtered_text = ' '.join(filtered_tokens)
		return filtered_text