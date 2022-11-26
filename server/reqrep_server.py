import time
import random
import zmq
from utils_nlp import analyze_sentence_eliza
import nltk

context = zmq.Context()
socket = context.socket(zmq.REP)
socket.bind("tcp://*:12345")

while True:
    #  wait request from client
    message_rx = socket.recv()
    message_rx = message_rx.decode("utf-8")
    sentence_list = nltk.sent_tokenize(message_rx);

    print(f"Received request: {message_rx}")
    #  reply to client
    message = analyze_sentence_eliza(sentence_list)
    print(message)
    socket.send_string(message)


