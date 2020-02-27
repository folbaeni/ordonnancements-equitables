#include "TestClass.h"

#include <iostream>
#include <string>

using namespace std;

TestClass::TestClass() {
    
}

void TestClass::print() {
    cout << "I am a test\n";
}

void TestClass::print(string s) {
    cout << s;
}