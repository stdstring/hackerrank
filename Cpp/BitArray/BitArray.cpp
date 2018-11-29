#include <iostream>
#include <vector>

const unsigned int maxValue = 1 << 31;
const unsigned int cellCount = maxValue / 8;

unsigned int generate_next(unsigned int prev, unsigned int p, unsigned int q)
{
    unsigned long long next = prev * p + q;
    return next % maxValue;
}

unsigned int use_number(std::vector<unsigned char> &storage, unsigned int number, unsigned int prevCount)
{
    unsigned int cellIndex = number / 8;
    unsigned int bitIndex = number % 8;
    unsigned char mask = 1 << bitIndex;
    if ((storage[cellIndex] & mask) == 0)
    {
        storage[cellIndex] = storage[cellIndex] | mask;
        return prevCount + 1;
    }
    return prevCount;
}

int main()
{
    unsigned int n, s, p, q;
    std::cin >> n >> s >> p >> q;
    std::vector<unsigned char> storage;
    storage.reserve(cellCount);
    unsigned int prev = s;
    unsigned int usedCount = use_number(storage, prev, 0);
    for (unsigned int iteration = 1; iteration < n; ++iteration)
    {
        unsigned int next = generate_next(prev, p, q);
        usedCount = use_number(storage, next, usedCount);
        prev = next;
    }
    std::cout << usedCount << std::endl;
    return 0;
}
