/*
* Crystal Home Systems
* Created by Austin and Ezra
* Open Source with Related GitHub Repo
* UNDER DEVELOPMENT
*
* Copyright© 2015 Austin VanAlstyne, Bailey Thorson
*/

/*
*This file is part of Crystal Home Systems.
*
*Crystal Home Systems is free software: you can redistribute it and/or modify
*it under the terms of the GNU General Public License as published by
*the Free Software Foundation, either version 3 of the License, or
*(at your option) any later version.
*
*Crystal Home Systems is distributed in the hope that it will be useful,
*but WITHOUT ANY WARRANTY; without even the implied warranty of
*MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*GNU General Public License for more details.
*
*You should have received a copy of the GNU General Public License
*along with Crystal Home Systems.  If not, see <http://www.gnu.org/licenses/>.
*/

#include "sysParams.h"
#include <stdlib.h>
#include <iostream>
#include <algorithm>
#include <string>
#include <windows.h>

#include <SFML/Audio.hpp>

using namespace std;
using namespace sf;

void fileOpenError();
string getCommand(char[]);
void systemStartupMessage();
void playMusic(char[]);
void analyzeCommand(string, bool);

Music music; // needs to be global for the playmusic function, cannot share var through passing

int main()
{
	bool running = true; 
	string command;

	// System Setup and Title
	systemStartupMessage();

	//play the startup song
	playMusic("test.ogg");

	// core system loop
	while (running){
		command = getCommand("Enter command here:");
		analyzeCommand(command, running);
	}

	getchar();
}

void fileOpenError(){
	cout << "Unable to open file!" << endl;
}

string getCommand(char c[]){
	cout << c << endl;
	string command;
	getline(cin, command);
	transform(command.begin(), command.end(), command.begin(), ::tolower);
	return command;
}

void systemStartupMessage(){
	SetConsoleTitle(TEXT("Crystal Home Systems")); // set console window title to Crystal Home System
	cout << systemName << " " << systemType << " Version " << version << "\nCreated By Ezra and Austin\n\n" << endl;
}

void playMusic(char dir[]){
	if (!music.openFromFile(dir))
		fileOpenError();
	music.play();
}

void analyzeCommand(string command, bool running){
	// finding command terms in the string. to add more commands to an if statement, enter: command.find("putCommandYouWandToUseHere") != string::npos
	if (command.find("exit") != string::npos || command.find("quit") != string::npos || command.find("close") != string::npos){ // user wants to quit
		if (command.find("song") != string::npos || command.find("music") != string::npos){
			music.stop();
			return;
		}
		if (command.find("movie") != string::npos || command.find("show") != string::npos){
			return;
		}
		running = false;
		music.stop();
		exit(0);
	}

	// stop command
	if (command.find("stop") != string::npos){
		if (command.find("song") != string::npos || command.find("music") != string::npos){
			music.stop();
			return;
		}
		if (command.find("movie") != string::npos || command.find("show") != string::npos){
			return;
		}
	}

	// play command
	if (command.find("play") != string::npos || command.find("start") != string::npos){ // wants to play media
		if (command.find("movie") != string::npos || command.find("show") != string::npos){ // movie

		}
		else if (command.find("song") != string::npos || command.find("music") != string::npos || command.find("album") != string::npos || command.find("artist") != string::npos){ // music
			// now that the loop as narrowed down that you want to play a song, make sure there's not another song already loaded, and then start the new song
			if (music.Paused){
				cout << "There is already a song playing. Are you sure you want to start a new one?" << endl;
				string temp;
				getline(cin, temp);
				transform(temp.begin(), temp.end(), temp.begin(), ::tolower);
				if (temp.find("no") != string::npos){ // dont play new song
					music.play();
					return;
				}
				else if (temp.find("yes") != string::npos){
					// LOAD SONG HERE
					return;
				}
				cout << "Please enter yes or no!" << endl;
				return;
			}
		}
	}

	// pause command
	if (command.find("pause") != string::npos){
		if (command.find("music") != string::npos || command.find("song") != string::npos){
			music.pause();
		}
		if (command.find("movie") != string::npos || command.find("show") != string::npos || command.find("tv") != string::npos){

		}
	}

	// resume command
	if (command.find("resume") != string::npos){
		if (command.find("music") != string::npos || command.find("song") != string::npos){
			if (music.Paused){
				music.play();
				return;
			}
			else{
				cout << "There is no song to resume!" << endl;
				return;
			}
		}
	}
}