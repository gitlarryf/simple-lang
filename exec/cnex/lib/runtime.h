#ifndef RUNTIME_H
#define RUNTIME_H

struct tagTExecutor;

void runtime_assertionsEnabled(struct tagTExecutor *exec);
void runtime_executorName(struct tagTExecutor *exec);
void runtime_isModuleImported(struct tagTExecutor *exec);
void runtime_garbageCollect(struct tagTExecutor *exec);
void runtime_getAllocatedObjectCount(struct tagTExecutor *exec);
void runtime_moduleIsMain(struct tagTExecutor *exec);
void runtime_setGarbageCollectionInterval(struct tagTExecutor *exec);
void runtime_setRecursionLimit(struct tagTExecutor *exec);
#endif
