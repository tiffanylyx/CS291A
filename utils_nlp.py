from sympy import *

from gensim.test.utils import common_texts
from gensim.models import Word2Vec

from sklearn.decomposition import PCA

import pickle

import numpy as np

import nltk
from nltk import *


from gensim.test.utils import common_texts
from gensim.models.doc2vec import Doc2Vec, TaggedDocument

from textblob import TextBlob

import random

from math import sqrt


import transformers

import torch.nn.functional as F
import os
import torch

import time
import json

from nltk.corpus import cmudict

# This function is used to clean the sentence
def pre_process_sentence(sentence):
    tokens = nltk.tokenize.word_tokenize(sentence)
    # lowercase
    tokens = [token.lower() for token in tokens]
    # isword
    tokens = [token for token in tokens if token.isalpha()]
    clean_sentence = ''
    clean_sentence = ' '.join(token for token in tokens)
    sentence1 = TextBlob(clean_sentence)
    sentence2 = str(sentence1.correct())
    return sentence2

def mean_pooling(model_output, attention_mask):
    token_embeddings = model_output[0] #First element of model_output contains all token embeddings
    input_mask_expanded = attention_mask.unsqueeze(-1).expand(token_embeddings.size()).float()
    return torch.sum(token_embeddings * input_mask_expanded, 1) / torch.clamp(input_mask_expanded.sum(1), min=1e-9)

csv_model = transformers.AutoModel.from_pretrained('sentence-transformers/all-MiniLM-L6-v2')
csv_tokenizer = transformers.AutoTokenizer.from_pretrained('sentence-transformers/all-MiniLM-L6-v2')

# This function is used to compute 3D sentence vector
def compute_sent_vec(sentence, pca3_sentenceVec):
    encoded_input = csv_tokenizer(sentence, padding=True, truncation=True, return_tensors='pt')
    with torch.no_grad():
        model_output = csv_model(**encoded_input)
    sentence_embeddings = mean_pooling(model_output, encoded_input['attention_mask'])
    sentence_embeddings = F.normalize(sentence_embeddings, p=2, dim=1)
    res = pca3_sentenceVec.transform(sentence_embeddings)[0]
    normalized_res = res/np.linalg.norm(res)
    return normalized_res


# This function is used to compute each word's vector in a sentence
def compute_word_vec_in_sentence(sentence, model_word, pca2, pca3, pca4, dim):
    tokens = nltk.tokenize.word_tokenize(sentence)
    res_vec = []
    for word in tokens:
        try:
            vector = model_word.wv[word]
            if dim==2:
                res = pca2.transform([vector])[0]
            elif dim==3:
                res = pca3.transform([vector])[0]
            elif dim==4:
                res = pca4.transform([vector])[0]
        except:
            res = [random.random(),random.random(),random.random()]
        normalized_res = res/np.linalg.norm(res)
        res_vec.append(normalized_res.tolist())

    return res_vec

# This sentence is to compute the Parts of Speech
def compute_sent_parts(sentence):
    tokens = nltk.tokenize.word_tokenize(sentence)
    res = nltk.pos_tag(tokens,tagset='universal')
    noun_count = 0
    verb_count = 0
    for i in res:
        if i[1]=='NOUN':
            noun_count+=1
        elif i[1]=='VERB':
            verb_count+=1
    return noun_count,verb_count

# To compute the word length
def compute_word_length(word):
    return len(word)

# To compute the sentence sentiment value
def compute_sent_sentiment(sentence):
    res = TextBlob(sentence)
    sentiment = res.sentiment.polarity
    sentiment = (sentiment + 1)/2
    return sentiment

def compute_sent_length(sentence):
    tokens = nltk.tokenize.word_tokenize(sentence)
    return len(tokens)

def flat(nums):
    count = 0
    res = []
    for i in nums:
        if isinstance(i, list):
            count+=1
            res.extend(flat(i))
        else:
            res.append(i)

    return res

# To compute the sentence structure based on the grammer
def get_cfg_structure(sentence):
    CFG_string = """
    S -> NP VP
    VP -> V NP | V NP PP
    PP -> P NP | V ADJ
    NP -> Det N | Det N PP | N
    """
    N_string = "N -> "
    P_string = "P -> "
    Det_string = "Det -> "
    V_string = "V -> "
    ADJ_string = "ADJ -> "
    sent = nltk.tokenize.word_tokenize(sentence)

    res = nltk.pos_tag(sent,tagset='universal')
    for i in res:
        if i[1] == "NOUN":
            N_string+="\"" + i[0]+"\""  + "|"
        elif i[1] == "PRON":
            N_string+="\"" + i[0]+"\""  + "|"
        elif i[1] == "DET":
            Det_string+="\"" + i[0]+"\""  + "|"
        elif i[1] == "VERB":
            V_string+="\"" + i[0]+"\""  + "|"
        elif i[1] == "ADP":
            P_string+="\"" + i[0]+"\""  + " |"
        elif i[1] == "ADJ":
            ADJ_string+="\"" + i[0]+"\""  + " |"

    CFG_string = CFG_string + N_string + "\n" + P_string + "\n" + Det_string +"\n" +  V_string + "\n" + ADJ_string
    grammar1 = CFG.fromstring(CFG_string)
    sent_clean = []
    for i in sent:
        if "\""+i+"\"" in CFG_string:
            sent_clean.append(i)

    parser = nltk.ChartParser(grammar1)
    trees = list(parser.parse(sent_clean))
    res_count = []
    word_parts = []
    res_key = {}
    for tree in trees[:1]:
        for part in tree:

            res = flat(part)
            word_parts.append(res)
            for i in res:
                res_key[i] = []
                res_key[i].append(1)
            if len(res)>1:
                for sub_part in part:
                    res = flat(sub_part)
                    for i in res:
                        res_key[i].append(2)
                    if len(res)>1:
                        for sub_sub_part in sub_part:
                            res = flat(sub_sub_part)
                        for i in res:
                            res_key[i].append(3)


    return word_parts,res_key
'''
def compute_co_reference(sentence):
    prediction = predictor.predict(document=sentence)  # get prediction
    return prediction['clusters']
'''

# to compute the syllables of each word in a sentence
def compute_syllables(sentence,d):
    tokens = nltk.tokenize.word_tokenize(sentence)
    res_all = []
    for word in tokens:
        try:
            res = d[word.lower()]
            list_res = []
            for y in res[0]:
                if y[-1].isdigit():
                    list_res.append(count)

        except:
            list_res = [word]
        res_all.append(list_res)
    return res_all

d = cmudict.dict()
script_dir = os.path.dirname(__file__)
with open(os.path.join(script_dir, 'model/pca4.pkl'), 'rb') as pickle_file:
    pca4 = pickle.load(pickle_file)
with open(os.path.join(script_dir, 'model/pca2.pkl'), 'rb') as pickle_file:
    pca2 = pickle.load(pickle_file)
with open(os.path.join(script_dir, 'model/pca3.pkl'), 'rb') as pickle_file:
    pca3 = pickle.load(pickle_file)
with open(os.path.join(script_dir, 'model/pca3_sentenceVec_transformer.pkl'), 'rb') as pickle_file:
    pca3_sentenceVec = pickle.load(pickle_file)

model_word = Word2Vec.load(os.path.join(script_dir, "model/word2vec_text8.model"))

def analyze_sentence(sentence_list):
    res_all_sentence = {}
    for index,sentence in enumerate(sentence_list):
        res = main_analyzer(sentence)
        res_all_sentence[index] = res

    return json.dumps(res_all_sentence)

def main_analyzer(sentence):
    all_result = {}
    sentence = pre_process_sentence(sentence)
    syllables = compute_syllables(sentence,d)
    #cfg = get_cfg_structure(sentence)
    senti = compute_sent_sentiment(sentence)
    noun_count,verb_count = compute_sent_parts(sentence)

    word_vec =  compute_word_vec_in_sentence(sentence, model_word, pca2, pca3, pca4, 3)

    sent_vec = compute_sent_vec(sentence, pca3_sentenceVec)
    all_result['syllables'] = len(syllables)
    #all_result['cfg'] = cfg
    all_result['senti'] = senti
    #all_result['pos'] = pos
    #all_result['word_vec'] = word_vec
    all_result['sent_vec'] = [vec.tolist() for vec in sent_vec]
    all_result['sentence'] = sentence
    all_result['sentence_length'] = compute_sent_length(sentence)
    all_result['noun'] = noun_count
    all_result['verb'] = verb_count


    return all_result


if __name__ == '__main__':
    sentence_list = ['this is a demo', 'i am happy to meet you today', 'i went to the museum with my best friend','it is a beautiful day for me']
    res = analyze_sentence(sentence_list)

    print(res)
