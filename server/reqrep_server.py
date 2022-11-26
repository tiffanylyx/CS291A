import json
import time
import random
import zmq

from dialog import create_chatbot, DIALOG_KEY
from utils_nlp import analyze_sentence
import nltk

context = zmq.Context()
socket = context.socket(zmq.REP)
socket.bind("tcp://*:12345")

bot = create_chatbot()

while True:
    #  wait request from client
    message_rx = socket.recv()
    message_rx = message_rx.decode("utf-8")
    sentence_list = nltk.sent_tokenize(message_rx);

    print(f"Received request: {message_rx}")

    #  reply to client
    data = analyze_sentence(sentence_list)
    print(data)

    # add chatbot response
    data[DIALOG_KEY] = bot(sentence_list)

    json_data = json.dumps(data)
    socket.send_string(json_data)
