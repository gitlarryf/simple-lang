#include "array.h"

#include <assert.h>
#include <stdarg.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "cell.h"
#include "util.h"

Array *array_createArray(void)
{
    Array *a = malloc(sizeof(Array));
    if (a == NULL) {
        fatal_error("Unable to allocate array.");
    }

    a->size = 0;
    a->data = NULL;
    return a;
}

Array *array_createArrayFromSize(size_t iElements)
{
    Array *a = array_createArray();

    a->size = iElements;
    a->data = malloc(sizeof(Cell) * a->size);
    if (a->data == NULL) {
        fatal_error("Unable to allocate memory for %d array elements.", a->size);
    }
    for (size_t i = 0; i < a->size; i++) {
        cell_resetCell(&a->data[i]);
    }
    return a;
}

void array_freeArray(Array *self)
{
    if (self != NULL && self->data != NULL) {
        for (size_t i = 0; i < self->size; i++) {
            cell_clearCell(&self->data[i]);
        }
        free(self->data);
    }
    free(self);
}

Array *array_copyArray(Array *a)
{
    Array *r = array_createArrayFromSize(a->size);

    for (size_t i = 0; i < r->size; i++) {
        cell_copyCell(&r->data[i], &a->data[i]);
    }
    return r;
}

size_t array_addArrayElement(Array *a, Cell *element)
{
    a->size++;
    a->data = realloc(a->data, sizeof(Cell) * a->size);
    if (a->data == NULL) {
        fatal_error("Could not allocate memory for appended array element, element num: %d", a->size);
    }

    cell_resetCell(&a->data[a->size-1]);
    cell_copyCell(&a->data[a->size-1], element);
    return a->size;
}
