from nltk.chat.eliza import eliza_chatbot

PROMPTS = (
    "Who is in this picture?",
    "Who took this picture?",
    "What is in this picture?",
    "When was this picture taken?",
    "Where was this picture taken?",
    "How do you feel about this picture?",
    "Could you tell me more about the story behind this picture?",
)

DIALOG_KEY = 'dialog'


def create_chatbot():
    prompts = iter(PROMPTS)

    def respond(text):
        reply = next(prompts, None)
        if reply:
            return reply
        return eliza_chatbot.respond(text)

    return respond
