#include <algorithm>
#include <cctype>
#include <functional>
#include <iostream>
#include <memory>
#include <stack>
#include <string>
#include <sstream>
#include <stdexcept>
#include <vector>

class Attribute
{
public:
    Attribute(std::string const &name, std::string const &value) : _name(name), _value(value) {}

    std::string const& GetName() const { return _name; }
    std::string const& GetValue() const { return _value; }

private:
    std::string _name;
    std::string _value;
};

enum TokenType { WHITESPACE, OPENING_TAG, CLOSING_TAG };

class Token
{
public:
    virtual TokenType GetTokenType() const = 0;
};

class Whitespace : public Token
{
public:
    TokenType GetTokenType() const override { return TokenType::WHITESPACE; }
};

class OpeningTag : public Token
{
public:
    OpeningTag(std::string const &name, std::vector<Attribute> const &attributes) : _name(name), _attributes(attributes) {}

    TokenType GetTokenType() const override { return TokenType::OPENING_TAG; }
    std::string const& GetName() const { return _name; }
    std::vector<Attribute>& GetAttributes() { return _attributes; }

private:
    std::string _name;
    std::vector<Attribute> _attributes;
};

class ClosingTag : public Token
{
public:
    ClosingTag(std::string const &name) : _name(name) {}

    TokenType GetTokenType() const override { return TokenType::CLOSING_TAG; }
    std::string const& GetName() const { return _name; }

private:
    std::string _name;
};

class IParser
{
public:
    virtual std::shared_ptr<Token> Parse(std::stringstream &source) = 0;
};

class WhitespaceParser : public IParser
{
public:
    virtual std::shared_ptr<Token> Parse(std::stringstream &source) override
    {
        while (source.good() && isspace(source.peek()))
        {
            source.get();
        }
        return std::shared_ptr<Token>(new Whitespace());
    }
};

std::string ExtractName(std::stringstream &source, std::function<bool(char)> predicate)
{
    std::string name;
    while (predicate(source.peek()))
    {
        char ch;
        source >> ch;
        name.push_back(ch);
    }
    return name;
}

void SkipWhitespaces(std::stringstream &source)
{
    while (isspace(source.peek()))
    {
        source.get();
    }
}

std::string ExtractValue(std::stringstream &source)
{
    const char borderChar = '"';
    char firstChar;
    source >> firstChar;
    std::string value;
    while (source.peek() != borderChar)
    {
        value.push_back(source.get());
    }
    source.get();
    return value;
}

class OpeningTagParser : public IParser
{
public:
    virtual std::shared_ptr<Token> Parse(std::stringstream &source) override
    {
        const char tagLastChar = '>';
        std::string name = ExtractName(source, [tagLastChar](char ch){ return ch != tagLastChar && !isspace(ch); });
        // extract attributes
        std::vector<Attribute> attributes;
        while (source.peek() != tagLastChar)
        {
            SkipWhitespaces(source);
            std::string attributeName = ExtractName(source, [](char ch){ return !isspace(ch); });
            SkipWhitespaces(source);
            char keyValueDelimiter;
            source >> keyValueDelimiter;
            SkipWhitespaces(source);
            std::string attributeValue = ExtractValue(source);
            attributes.push_back(Attribute(attributeName, attributeValue));
        }
        source.get();
        return std::shared_ptr<Token>(new OpeningTag(name, attributes));
    }
};

class ClosingTagParser : public IParser
{
public:
    virtual std::shared_ptr<Token> Parse(std::stringstream &source) override
    {
        const char tagLastChar = '>';
        std::string name = ExtractName(source, [tagLastChar](char ch){ return ch != tagLastChar; });
        source.get();
        return std::shared_ptr<Token>(new ClosingTag(name));
    }
};

class Node
{
public:
    Node(std::string const &name, std::vector<Attribute> const &attributes) : _name(name), _attributes(attributes) {}

    std::string const& GetName() const { return _name; }
    std::vector<Attribute>& GetAttributes() { return _attributes; }
    std::vector<std::shared_ptr<Node>>& GetChildren() { return _children; }

private:
    std::string _name;
    std::vector<Attribute> _attributes;
    std::vector<std::shared_ptr<Node>> _children;
};

std::shared_ptr<IParser> ChooseParser(std::stringstream &source)
{
    const char tagFirstChar = '<';
    const char closingTagSecondChar = '/';
    if (isspace(source.peek()))
        return std::shared_ptr<IParser>(new WhitespaceParser());
    if (source.peek() == tagFirstChar)
    {
        source.get();
        if (source.peek() == closingTagSecondChar)
        {
            source.get();
            return std::shared_ptr<IParser>(new ClosingTagParser());
        }
        else
            return std::shared_ptr<IParser>(new OpeningTagParser());
    }
    throw std::logic_error("Unknown parser");
}

std::vector<std::shared_ptr<Node>> Parse(std::stringstream &source)
{
    std::stack<std::shared_ptr<Node>> treeStack;
    std::vector<std::shared_ptr<Node>> rootNodes;
    while (source.peek() != -1)
    {
        std::shared_ptr<IParser> parser = ChooseParser(source);
        std::shared_ptr<Token> token = parser.get()->Parse(source);
        switch (token.get()->GetTokenType())
        {
            case TokenType::WHITESPACE:
                // do nothing;
                break;
            case TokenType::OPENING_TAG:
            {
                OpeningTag *openingTag = dynamic_cast<OpeningTag*>(token.get());
                std::shared_ptr<Node> node(new Node(openingTag->GetName(), openingTag->GetAttributes()));
                if (treeStack.empty())
                {
                    rootNodes.push_back(node);
                }
                else
                {
                    treeStack.top().get()->GetChildren().push_back(node);
                }
                treeStack.push(node);
                break;
            }
            case TokenType::CLOSING_TAG:
                // probably check tag name
                treeStack.pop();
                break;
            default:
                throw std::logic_error("Unknown token");
        }
    }
    return rootNodes;
}

std::shared_ptr<Node> FindNodeByName(std::vector<std::shared_ptr<Node>> const &nodes, std::string const &name)
{
    std::vector<std::shared_ptr<Node>>::const_iterator iterator = std::find_if(nodes.cbegin(), nodes.cend(), [&name](std::shared_ptr<Node> node){ return node.get()->GetName() == name; });
    return (iterator == nodes.cend()) ? std::shared_ptr<Node>() : *iterator;
}

std::string FindPath(std::string const &path, std::vector<std::shared_ptr<Node>> const &rootNodes)
{
    const char* notFoundResult = "Not Found!";
    std::vector<std::string> parts;
    std::string::const_iterator iterator = path.cbegin();
    while (iterator != path.cend())
    {
        std::string::const_iterator nextIterator = std::find(iterator, path.cend(), '.');
        parts.push_back(std::string(iterator, nextIterator));
        iterator = nextIterator;
        if (iterator != path.cend())
            ++iterator;
    }
    std::string attributeName;
    std::string &lastSegment = parts.back();
    std::string::const_iterator lastSegmentIterator = std::find(lastSegment.cbegin(), lastSegment.cend(), '~');
    if (lastSegmentIterator != lastSegment.cend())
    {
        std::string updatedLastSegment(lastSegment.cbegin(), lastSegmentIterator);
        ++lastSegmentIterator;
        attributeName = std::string(lastSegmentIterator, lastSegment.cend());
        parts.back() = updatedLastSegment;
    }
    // first segment
    std::shared_ptr<Node> current = FindNodeByName(rootNodes, parts.at(0));
    if (!current)
        return notFoundResult;
    // segments
    for (unsigned int index = 1; index < parts.size(); ++index)
    {
        current = FindNodeByName(current.get()->GetChildren(), parts.at(index));
        if (!current)
            return notFoundResult;
    }
    if (attributeName.empty())
        return current.get()->GetName();
    // attribute
    std::vector<Attribute>& attributes = current.get()->GetAttributes();
    std::vector<Attribute>::const_iterator attributeIterator = std::find_if(attributes.cbegin(), attributes.cend(), [&attributeName](Attribute attribute){ return attribute.GetName() == attributeName; });
    if (attributeIterator != attributes.cend())
        return (*attributeIterator).GetValue();
    return notFoundResult;
}

int main()
{
    int n, q;
    std::cin >> n >> q;
    std::string line;
    std::stringstream stream;
    // remove "/r/n" after "std::cin >> n >> q"
    std::getline(std::cin, line);
    for (int i = 0; i < n; ++i)
    {
        std::getline(std::cin, line);
        stream << line;
    }
    std::vector<std::shared_ptr<Node>> rootNodes = Parse(stream);
    for (int i = 0; i < q; ++i)
    {
        std::string path;
        std::cin >> path;
        std::cout << FindPath(path, rootNodes) << std::endl;
    }
    return 0;
}