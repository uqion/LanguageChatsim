### Sphinx, Watson, Dialogflow and general speech recognition efforts report

#### Watson
After reading through documentation and various forums, it seems that our original thoughts
on how much better Watson would be over Azure were misguided. Watson speech-to-text does 
have a more powerful API in that it provides multiple alternative results for recognition,
but the granularity within those results are the same (only confidence values are provided)
    * Our original conception of how useful Watson would be came from a comment that Daniel
    made confirming Watson's ability to recognize phonemes. It is possible that I have not dug
    deep enough to find what he was referencing, or the behaviour have been deprecated. TALK TO DANIEL

#### Sphinx
Spending several hours reading about what sphinx can do has shown me that it is a VERY powerful tool,
if used properly. Sphinx provides the user with a GENERAL model for speech recognition (build inside
is a hidden markov model and multidimensional gaussian distribution), that takes input in the form 
of a phoneme dictionary and codification of an arbitrary number of words as well as optional grammar
alignment codification (phone alignment as well as word alignment), and runs input utterances (sound files)
through the model to output an acousic score for each phoneme in the utterance (this tells us how likely the aligned
phone is to be the coded phone from the dictionary). In this tutorial: https://cmusphinx.github.io/wiki/pocketsphinx_pronunciation_evaluation/
there is an example of how to use this process combined potentially with a logistic regression model (or anoterh ML technique) with the 
acoustic scores to output an intellibility score. This is just one example of what can be done, and I am sure that
with more experience using sphinx a more complex system can be produced that outputs the values that we want.
