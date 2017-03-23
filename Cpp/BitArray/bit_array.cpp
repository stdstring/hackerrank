#include <iostream>
#include <memory>
#include <cstring>

const unsigned int maxValue = 1 << 31;
const unsigned int cellCount = maxValue / 8;

unsigned int generate_next(unsigned int prev, unsigned int p, unsigned int q)
{
    unsigned long long next = prev * p + q;
    return next % maxValue;
}

unsigned int use_number(std::shared_ptr<unsigned char> &storage, unsigned int number, unsigned int prevCount)
{
    unsigned int cellIndex = number / 8;
    unsigned int bitIndex = number % 8;
    unsigned char mask = 1 << bitIndex;
    if ((storage.get()[cellIndex] & mask) == 0)
    {
        storage.get()[cellIndex] = storage.get()[cellIndex] | mask;
        return prevCount + 1;
    }
    return prevCount;
}

int main()
{
    unsigned int n, s, p, q;
    std::cin >> n >> s >> p >> q;
    std::shared_ptr<unsigned char> storage(new unsigned char[cellCount], std::default_delete<unsigned char[]>());
    memset(storage.get(), 0, sizeof(unsigned char) * cellCount);
    unsigned int prev = s;
    unsigned int usedCount = use_number(storage, prev, 0);
    for (int i = 1; i < n; ++i)
    {
        unsigned int next = generate_next(prev, p, q);
        usedCount = use_number(storage, next, usedCount);
        prev = next;
    }
    std::cout << usedCount << std::endl;
    return 0;
}
