
print('hello')


deleted
def word_drop(text):
			#text=text.lower()
			text=re.sub('\[.*?\]','',text)
			text = re.sub(r'[^\w\s]', '', text) #remove punctuation
			text=re.sub("\\W","",text)
			text=re.sub('https?://\S+|www\.\S+','',text)
			text=re.sub('<.*?>+','',text)
			text=re.sub('[%s]' % re.escape(string.punctuation),'',text)
			text = re.sub(r'\s+', ' ', text).strip() # remove white space and trim
			text=re.sub('\n','',text) #Remove New lines
			
			text=re.sub('\w"\d\w"','',text)
			#normalized_tokens
			stop_words = nltk.corpus.stopwords.words('english')
			text = [word for word in text.split() if word not in stop_words]# Remove stop words
			# tokens = nltk.word_tokenize(text)
			# normalized_tokens = [nltk.stem.PorterStemmer().stem(token) for token in tokens]# Normalize the text
			return text
		
		#text=df["text"].apply(word_drop)