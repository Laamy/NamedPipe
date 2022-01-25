#pragma region Includes

// System
#include <iostream>
#include <windows.h>
#include <stdio.h>
#include <conio.h>
#include <tchar.h>
#include <vector>

// Utils
#include "Utils.h"

#pragma endregion

void sendData(HANDLE pipe, BOOL result, const wchar_t* data) {
	printL("Sending data to pipe...");

	DWORD numBytesWritten = 0;
	result = WriteFile(pipe, data, wcslen(data) * sizeof(wchar_t), &numBytesWritten, 0);

	if (result) printL("sent");
	else printL("Failed to send data.");
	Sleep(2000);
}

int main() {
	HANDLE pipe = CreateNamedPipe(L"\\\\.\\pipe\\PMServer-Cl2Dkes", PIPE_ACCESS_OUTBOUND, PIPE_TYPE_BYTE, 1, 0, 0, 0, 0);

	if (pipe == NULL || pipe == INVALID_HANDLE_VALUE) {
		printL("Failed to create outbound pipe instance.");
		stop();
	}

	printL("Waiting for a client to connect to the pipe...");

	BOOL result = ConnectNamedPipe(pipe, NULL);
	if (!result) {
		printL("Failed to make connection on named pipe.");
		CloseHandle(pipe);
		system("pause");
	}

	sendData(pipe, result, L"correct packet");

	CloseHandle(pipe);

	printL("Done.");
	stop();
}