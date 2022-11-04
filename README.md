# This is the Python code of the CS291A project

## Procedures

```
$ git clone https://github.com/tiffanylyx/CS291A.git

$ cd CS291A
```
Then copy the files from the google drive to the folder "/model"

```
$ pip install -r requirements.txt
```
Then install the dependencies from nltk
```
$ python install_nltk_file.py
```
You can run the following command to test
```
$ python utils_nlp.py
```
For an input sentence, you can just call the function main(sentence, pca2, pca3, pca4, pca3_sentenceVec, model_word, d) in utils_nlp.py. Arguments except 'sentence' are preloaded models that gonna be used in the calculation. The main() function will return a dictionary that store all the calculated results. We can save the result as JSON and send it back to Unity.
