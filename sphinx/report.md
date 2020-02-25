### Sphinx, Watson, Dialogflow and general speech recognition efforts report Jan 6th - 21st

#### Watson
After reading through documentation and various forums, we have found that Watson speech-to-text does
have a more powerful API in that it provides multiple alternative results for recognition, and
confidence values for individual words as opposed to the whole utterance.
    * All in all, switching to Watson will give us more information to work towards constructive 
	feedback for the learner which I believe will work towards fulfilling aspect 2 (and perhaps 3) of POC 2.0

As of now no additional behaviour for the chatbot has been implemented, but our usage of Azure recognition has been swapped out
for Watson's basic whole utterance transcript functionality.

#### Sphinx
Spending several hours reading about what sphinx can do has shown me that it is a VERY powerful tool,
if used properly. Sphinx provides the user with a GENERAL model for speech recognition (built inside
is a hidden markov model and multidimensional gaussian distribution), that takes input in the form
of a phoneme dictionary and codification of an arbitrary number of words in this phoneme dictionary as well as optional grammar
alignment codification (phone alignment as well as word alignment), and runs input utterances (sound files)
through the model to output an acoustic score for each phoneme in the utterance (this tells us how likely the aligned
phone is to be the same as the coded phone from the dictionary). In this tutorial: https://cmusphinx.github.io/wiki/pocketsphinx_pronunciation_evaluation/
there is an example of how to use this process combined potentially with a logistic regression model (or another ML technique) using
acoustic scores to output an intelligibility score. This is just one example of what can be done, and I am sure that
with more experience using sphinx a more complex system can be developed that outputs the values that we want.
    * Using sphinx will be a learning process, as I am not deeply familiar with the techniques required, but it also poses
    an opportunity for collaboration with the department of computer science (this stuff is right up their ally) as well
    as some justification for additional funding for more talent.
    * I recommend that stakeholders take a look at the link in the above paragraph. I understand there is a lot
    of technical details, but taking 30 minutes to read through the explanation will show you how general sphinx actually
    is, and may give you some ideas as to how we can use it.

Currently we are working through sphinx examples and setting up a development environment for it. This development will be done in parallel
to the development of chatbot functionality, with completion of the POC aspects taking priority.


#### Dialogflow
We have finished migrating the code that we were using to talk to the dialogflow agent from the v1 API (deprecated on march 31st 2020)
to the v2 API. The behaviour of the agent remains the same.

#### Text to speech
* We are still using azure cognitive services for the text-to-speech portion of the functionality, this behaviour already existed,
the only change is that the input now comes from dialogflow intent fulfillment.
* currently, SALSA is being used for facial animations

#### Moving forward:
* Since we will be using more complex animations to sell the scene to the user, we will need a system that will orchestrate audio and animations
feedback
  * we will be using scriptable objects and unity timeline.  With these components combined with with text-to-speech portion we will make an extendable
  framework for chatbot feedback.
