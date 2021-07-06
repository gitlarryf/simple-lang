#ifndef OPSTACK_H
#define OPSTACK_H

#include <vector>

template <typename T> class opstack {
public:
    opstack(): a() {}
    typename std::vector<T>::reverse_iterator begin() { return a.rbegin(); }
    size_t depth() const { return a.size(); }
    void drop(size_t n) { a.resize(a.size() - n); }
    bool empty() const { return a.empty(); }
    typename std::vector<T>::reverse_iterator end() { return a.rend(); }
    T &peek(size_t n) { return a.at(a.size() - 1 - n); }
    void push(const T &x) { a.push_back(x); }
    void pop() { a.pop_back(); }
    T &top() { return a.back(); }
private:
    std::vector<T> a;
};

#endif
