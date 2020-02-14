#include "gc.h"

#include <limits.h>
#include <stdlib.h>

#include "bytecode.h"
#include "cell.h"
#include "exec.h"
#include "framestack.h"
#include "module.h"
#include "nstring.h"
#include "stack.h"
#include "util.h"

#include <stdio.h>
#include <string.h>

//#define SKIP_COLLECTED_OBJECTS

void heap_allocObject(TExecutor *exec, Cell *p)
{
    TAllocationObject *o = malloc(sizeof(TAllocationObject));
    if (o == NULL) {
        fatal_error("Failed to allocate heap object after %ul allocations.", exec->allocations);
    }
    exec->diagnostics.total_allocations++;

    o->allocNum = exec->diagnostics.total_allocations;
    o->pointer = p;
    o->pointer->alloced = TRUE;
    o->next = NULL;

    // Push allocated object on the virtual heap.
    if (exec->firstObject != NULL) {
        o->next = exec->firstObject;
    }
    exec->firstObject = o;
}

unsigned int heap_getObjectCount(TExecutor *exec)
{
    // Count all allocated objects, regardless if they are in use or not.
    unsigned int ret = 0;
    TAllocationObject *object = exec->firstObject;

    while (object) {
        ret++;
        object = object->next;
    }
    return ret;
}

void heap_markAll(TExecutor *exec)
{
    // Walk the list of allocated objects, and clear the marked flag.
    TAllocationObject *object = exec->firstObject;
    while (object) {
        //cell_unmarkCell(object->pointer);
        object->pointer->marked = FALSE;
        object = object->next;
    }

    // Mark local variables in all modules
    for (size_t m = 0; m < exec->module_count; m++) {
        fprintf(stderr, "Marking Module: %zd (%s)\n", m, exec->modules[m]->name);
        for (size_t v = 0; v < exec->modules[m]->bytecode->global_size; v++) {
            fprintf(stderr, "\tMarking Module Global %zd\n", v);
            //exec->modules[m]->globals[v].marked = 1;
            cell_markCell(&exec->modules[m]->globals[v]);
        }
    }

    // Mark Frame Stacks
    for (size_t f = 0; f < exec->framestack->top + 1; f++) {
        fprintf(stderr, "Marking Framestack: %zd\n", f);
        for (size_t l = 0; l < exec->framestack->data[f]->frame_size; l++) {
            fprintf(stderr, "\tMarking Framestack: %zd, Local: %zd\n", f, l);
            //exec->framestack->data[f]->locals[l].marked = 1;
            cell_markCell(&exec->framestack->data[f]->locals[l]);
        }
    }

    // Mark the opstack...
    for (int x = 0; x < exec->stack->top + 1; x++) {
        fprintf(stderr, "Marking opstack: %d\n", x);
        //exec->stack->data[x]->marked = 1;
        cell_markCell(exec->stack->data[x]);
    }
}

#if 0
static void heap_resetObjects(TExecutor *exec)
{
    // Walk the list of allocated objects, and clear the marked flag.
    TAllocationObject *object = exec->firstObject;
    while (object) {
        //cell_unmarkCell(object->pointer);
        object->pointer->marked = FALSE;
        object = object->next;
    }
}
#endif

void heap_sweepHeap(TExecutor *exec, BOOL bDiagStats)
{
    uint64_t count = 0;
    uint64_t freed = 0;

#if 0
    // DEBUG: Test removing flags before marking, to see what is left behind after a reset.
    //        Any objects marked with the marked member set, is a problem!
    // Clear all marked flags
    heap_resetObjects(exec);
#endif
    // First step, mark all objects.
    heap_markAll(exec);

    // Next step, sweep and clear (free) all objects that aren't marked.
    TAllocationObject **object = &exec->firstObject;
    while (*object) {
        count++;
        if ((*object)->pointer->marked == FALSE) {
            // This object wasn't reached, so remove it from the list then free it.
            if (bDiagStats) {
                printf("gc: Collecting AllocNum: %lld\n", (*object)->allocNum);
            }
#ifdef SKIP_COLLECTED_OBJECTS
            // If we aren't actually performing collections, then just move on to the next object.
            object = &(*object)->next;
#else
            TAllocationObject *unreached = *object;
            *object = unreached->next;
            cell_freeCell(unreached->pointer);
            free(unreached);
            exec->diagnostics.collected_objects++;
            freed++;
        } else {
            object = &(*object)->next;
#endif
        }
    }
    if (bDiagStats) {
        printf("GC: Scanned %lld objects, collected %lld objects\n", count, freed);
        //heap_dumpHeap(exec);
    }
    // Final step, reset our allocation count / GC cycle count
    exec->allocations = 0;
}

void heap_freeHeap(TExecutor *exec)
{
    // Free ALL objects on the heap, regardless if they are in use or not.
    // This function is performed on shudown, to free any and all objects 
    // that may still be allocated.
    TAllocationObject *object = exec->firstObject;
    uint64_t count = 0;

    while (object) {
        TAllocationObject *freeObj = object;
        object = freeObj->next;
        cell_freeCell(freeObj->pointer);
        free(freeObj);
        freeObj = NULL;
        count++;
    }
    exec->firstObject = NULL;
    if (exec->debug) {
        fprintf(stderr, "Freed %lld abandoned items.", count);
    }
}

void heap_dumpHeap(TExecutor *exec)
{
    // Dump ALL objects on the heap.
    TAllocationObject *object = exec->firstObject;
    while (object) {
        TAllocationObject *freeObj = object;
        fprintf(stderr, "Heap Object# %lld\n------------------\n", freeObj->allocNum);
        if (freeObj->pointer == NULL) {
            fprintf(stderr, "  NULL Cell Pointer!");
        } else {
            // ToDo: cell_toString() formatted output of the object data for diagnostic purposes.
            fprintf(stderr, "  Cell Type: %d", freeObj->pointer->type);
        }
        fprintf(stderr, "\n");
        object = freeObj->next;
    }
}

