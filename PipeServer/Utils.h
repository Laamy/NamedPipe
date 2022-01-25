#pragma once

void print(const char* str) { std::cout << str; }
void printL(const char* str) { print(std::string(str + std::string("\n")).c_str()); }
void stop() { system("pause"); }