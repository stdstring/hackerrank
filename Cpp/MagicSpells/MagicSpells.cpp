#include <algorithm>
#include <iostream>
#include <vector>
#include <string>

using namespace std;

class Spell {
private:
    string scrollName;
public:
    Spell() : scrollName("") { }
    Spell(string name) : scrollName(name) { }
    virtual ~Spell() { }
    string revealScrollName() {
        return scrollName;
    }
};

class Fireball : public Spell {
private: int power;
public:
    Fireball(int power) : power(power) { }
    void revealFirepower() {
        cout << "Fireball: " << power << endl;
    }
};

class Frostbite : public Spell {
private: int power;
public:
    Frostbite(int power) : power(power) { }
    void revealFrostpower() {
        cout << "Frostbite: " << power << endl;
    }
};

class Thunderstorm : public Spell {
private: int power;
public:
    Thunderstorm(int power) : power(power) { }
    void revealThunderpower() {
        cout << "Thunderstorm: " << power << endl;
    }
};

class Waterbolt : public Spell {
private: int power;
public:
    Waterbolt(int power) : power(power) { }
    void revealWaterpower() {
        cout << "Waterbolt: " << power << endl;
    }
};

class SpellJournal {
public:
    static string journal;
    static string read() {
        return journal;
    }
};
string SpellJournal::journal = "";

void counterspell(Spell *spell) {
    if (Fireball *fireball = dynamic_cast<Fireball*>(spell))
    {
        fireball->revealFirepower();
    }
    else if (Frostbite *frostbite = dynamic_cast<Frostbite*>(spell))
    {
        frostbite->revealFrostpower();
    }
    else if (Thunderstorm *thunderstorm = dynamic_cast<Thunderstorm*>(spell))
    {
        thunderstorm->revealThunderpower();
    }
    else if (Waterbolt *waterbolt = dynamic_cast<Waterbolt*>(spell))
    {
        waterbolt->revealWaterpower();
    }
    else
    {
        // from https://en.wikipedia.org/wiki/Longest_common_subsequence_problem
        std::string scroll = spell->revealScrollName();
        std::string journal = SpellJournal::read();
        int m = scroll.size();
        int n = journal.size();
        std::vector<int> prevRow;
        prevRow.resize(n + 1);
        std::vector<int> row;
        row.resize(n + 1);
        for (int j = 0; j <= n; ++j)
            row.at(j) = 0;
        for (int i = 1; i <= m; ++i)
        {
            row.swap(prevRow);
            row.at(0) = 0;
            for (int j = 1; j <= n; ++j)
            {
                if (scroll[i - 1] == journal[j -1])
                    row[j] = prevRow[j - 1] + 1;
                else
                    row[j] = std::max(row[j - 1], prevRow[j]);
            }
        }
        int value = row.at(n);
        std::cout << value << std::endl;
    }
}

class Wizard {
public:
    Spell *cast() {
        Spell *spell;
        string s; cin >> s;
        int power; cin >> power;
        if (s == "fire") {
            spell = new Fireball(power);
        }
        else if (s == "frost") {
            spell = new Frostbite(power);
        }
        else if (s == "water") {
            spell = new Waterbolt(power);
        }
        else if (s == "thunder") {
            spell = new Thunderstorm(power);
        }
        else {
            spell = new Spell(s);
            cin >> SpellJournal::journal;
        }
        return spell;
    }
};

int main() {
    int T;
    cin >> T;
    Wizard Arawn;
    while (T--) {
        Spell *spell = Arawn.cast();
        counterspell(spell);
    }
    return 0;
}