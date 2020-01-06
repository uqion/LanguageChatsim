#### Instructions for installing sphinx on windows computer:
1. install python: https://www.python.org/downloads/ (I did this with python 3.7.4)
2. follow instructions here: https://pypi.org/project/pocketsphinx/
    * wheel and setuptools should install fine, but before you can run pip instlal --upgrade pocketsphinx you will need to install swig, which can be found here: http://www.swig.org/download.html
    * swig installation is also not super easy, you can either put the swig files in your python installation
    or add to your environment variables like so: https://stackoverflow.com/questions/44504899/installing-pocketsphinx-python-module-command-swig-exe-failed
3. once you can run the first example in the pypi.org article, (need to use a microphone, will print out roughly what you are saying) the installation is completed
