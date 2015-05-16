/*
* Crystal Home Systems
* Created by Austin and Ezra
* Open Source with Related GitHub Repo
* UNDER DEVELOPMENT
*
* Copyrightę 2015 Austin VanAlstyne, Bailey Thorson
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

#include "stdafx.h"
#include "sysParams.h"
#include <iostream>
#include <string>
#include <sstream>
#include <windows.h>

using namespace std;

bool running = false;

int main()
{
	// System Setup and Title
	SetConsoleTitle(TEXT("Crystal Home Systems")); // set console window title to Crystal Home System
	cout << systemName << " " << systemType << " Version " << version << "\nCreated By Ezra and Austin\n\n" << endl; 
	runningP = &running; // set the global pointer running to the local version, for better mem management
	*runningP = true;

	// core system loop
	while (*runningP){
		// retrieve user input
		cout << "Enter a command:" << endl;
		string command;
		getline(cin, command);

		// finding command terms in the string. to add more commands to an if statement, enter: command.find("putCommandYouWandToUseHere") != string::npos
		if (command.find("exit") != string::npos || command.find("quit") != string::npos || command.find("close") != string::npos || command.find("stop") != string::npos){ // user wants to quit
			return 0;
		}
	}

	getchar();
	return 0;
}