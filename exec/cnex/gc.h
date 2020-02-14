#ifndef GC_H
#define GC_H
#include <stddef.h>
#include <stdint.h>
#include <util.h>

struct tagTExecutor;
struct tagTCell;

typedef struct tagTAllocationObject {
    uint64_t allocNum;
    struct tagTCell *pointer;
    struct tagTAllocationObject *next;
} TAllocationObject;

//typedef struct tagTAllocList {
//    struct tagTAllocList *prev;
//    TAlloc *alloc;
//    struct tagTAllocList *next;
//} TAllocList;

void heap_allocObject(struct tagTExecutor *exec, struct tagTCell *p);
unsigned int heap_getObjectCount(struct tagTExecutor *exec);
void heap_markAll(struct tagTExecutor *exec);
void heap_sweepHeap(struct tagTExecutor *exec, BOOL bDiagStats);
void heap_freeHeap(struct tagTExecutor *exec);
void heap_dumpHeap(struct tagTExecutor *exec);

//typedef struct tagTAllocList {
//    struct {
//        int top;
//        int capacity;
//        int max;
//        int len;
//        unsigned int *allocs;
//    } AllocList;
//
//    void (*push)(struct tagTAllocList *,unsigned int);
//    void (*pop)(struct tagTAllocList *);
//    size_t (*top)(struct tagTAllocList *);
//    size_t (*peek)(struct tagTAllocList *, int);
//    int (*isEmpty)(struct tagTAllocList *);
//} TAllocList;

//TAllocList *createAllocList(int capacity);
//void destroyAllocList(struct tagTAllocList *stack);
//
//void alloc_push(struct tagTAllocList *stack, unsigned int alloc);
//void alloc_pop(struct tagTAllocList *stack);
//unsigned int alloc_top(struct tagTAllocList *stack);
//unsigned int alloc_peek(struct tagTAllocList *stack, int depth);
//int allocList_isEmpty(struct tagTAllocList *stack);
//
//
//
#endif
