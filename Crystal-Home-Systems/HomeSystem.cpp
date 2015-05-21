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
#include "media.h"
#include "command.h"
#include "homeSystem.h"
#include <iostream>
#include <string>
#include <windows.h>

using namespace std;

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

void systemStartupMessage(){
	SetConsoleTitle(TEXT("Crystal Home Systems")); // set console window title to Crystal Home System
	cout << systemName << " " << systemType << " Version " << version << "\nCreated By Ezra and Austin\n\n" << endl;
}