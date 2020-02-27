#include <iostream>
#include "TestClass.h"

using namespace std;

int main(void) {
    std::cout << "Ta mere !\n" << std::endl;
    
    TestClass t;
    t.print();
    t.print("Coucou ali!\n");
    return 0;
}