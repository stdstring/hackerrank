#include <algorithm>
#include <iostream>
#include <vector>

// from https://en.wikipedia.org/wiki/Interval_scheduling : greedy solution
struct Workshop
{
public:
    Workshop(int startTime, int duration) : startTime(startTime), duration(duration), endTime(startTime + duration) {}

    int startTime, duration, endTime;
};

struct Available_Workshops
{
public:
    std::vector<Workshop> workshops;
};

Available_Workshops* initialize(int start_time[], int duration[], int n) 
{
    Available_Workshops *dest = new Available_Workshops();
    dest->workshops.reserve(n);
    for (int index = 0; index < n; ++index)
    {
        dest->workshops.push_back(Workshop(start_time[index], duration[index]));
    }
    auto comp = [](Workshop const &left, Workshop const &right)
    {
        return (left.endTime < right.endTime) || ((left.endTime == right.endTime) && (left.startTime < right.startTime));
    };
    std::sort(dest->workshops.begin(), dest->workshops.end(), comp);
    return dest;
}

int CalculateMaxWorkshops(Available_Workshops* ptr)
{
    int count = 0;
    int currentEndTime = -1;
    for (std::vector<Workshop>::const_iterator iterator = ptr->workshops.cbegin(); iterator < ptr->workshops.cend(); ++iterator)
    {
        if (currentEndTime <= iterator->startTime)
        {
            currentEndTime = iterator->endTime;
            ++count;
        }
    }
    return count;
}

// start of readonly code from hackerrank
int main(int argc, char *argv[]) {
    int n; // number of workshops
    std::cin >> n;
    // create arrays of unknown size n
    int* start_time = new int[n];
    int* duration = new int[n];

    for (int i = 0; i < n; i++) {
        std::cin >> start_time[i];
    }
    for (int i = 0; i < n; i++) {
        std::cin >> duration[i];
    }

    Available_Workshops * ptr;
    ptr = initialize(start_time, duration, n);
    std::cout << CalculateMaxWorkshops(ptr) << std::endl;
    return 0;
}
// end of readonly code from hackerrank