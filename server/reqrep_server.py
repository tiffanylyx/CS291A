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
    message = analyze_sentence(sentence_list)
    print(message)

    # add chatbot response
    message[DIALOG_KEY] = bot(sentence_list)

    socket.send_string(message)
