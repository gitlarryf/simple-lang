#include <assert.h>
#include <stdint.h>
#include <string.h>

#include "array.h"
#include "cell.h"
#include "dictionary.h"
#include "number.h"
#include "nstring.h"
#include "util.h"

#include "object.h"

Cell *object_toString(Object *obj)
{
    if (obj->type == oArray) {
        Cell *cell = cell_newCell();
        TString *r = string_createString(0);
        size_t x;
        Array *a = obj->pCell->array;
        r = string_appendCString(r, "[");
        for (x = 0; x < a->size; x++) {
            if (r->length > 1) {
                r = string_appendCString(r, ", ");
            }
            TString *es = cell_toString(a->data[x].object->pCell);
            r = string_appendString(r, es);
            string_freeString(es);
        }
        r = string_appendCString(r, "]");
        cell_setString(cell, r);
        return cell;
    } else if (obj->type == oBoolean) {
        if (obj->pCell->boolean == TRUE) {
            return cell_fromCString("TRUE");
        }
        return cell_fromCString("FALSE");
    } else if(obj->type == oBytes) {
        Cell *cell = cell_newCell();
        BOOL first = TRUE;
        TString *r = string_createCString("HEXBYTES \"");
        TString *bytes = obj->pCell->string;
        for (size_t x = 0; x < bytes->length; x++) {
            if (first) {
                first = FALSE;
            } else {
                r = string_appendCString(r, " ");
            }
            char buf[3];
            snprintf(buf, sizeof(buf), "%02x", bytes->data[x]);
            r = string_appendCString(r, buf);
        }
        r = string_appendCString(r, "\"");
        cell_setString(cell, r);
        return cell;
    } else if(obj->type == oDictionary) {
        Cell *cell = cell_newCell();
        TString *r = string_createString(0);
        size_t x;
        Dictionary *d = obj->pCell->dictionary;
        r = string_appendCString(r, "{");
        Cell *keys = dictionary_getKeys(d);
        for (x = 0; x < keys->array->size; x++) {
            if (r->length > 1) {
                r = string_appendCString(r, ", ");
            }
            r = string_appendCString(r, "\"");
            r = string_appendString(r, keys->array->data[x].string);
            r = string_appendCString(r, "\": ");
            TString *de = cell_toString(dictionary_findDictionaryEntry(d, keys->array->data[x].string)->object->pCell);
            r = string_appendString(r, de);
            string_freeString(de);
        }
        r = string_appendCString(r, "}");
        cell_freeCell(keys);
        cell_setString(cell, r);
        return cell;
    } else if(obj->type == oNumber) {
        return cell_fromCString(number_to_string(obj->pCell->number));
    } else if(obj->type == oString) {
        Cell *cell = cell_fromCString("\"");
        cell->string = string_appendString(cell->string, obj->pCell->string);
        cell->string = string_appendCString(cell->string, "\"");
        return cell;
    }
    return NULL;
}

Object *object_createObject()
{
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate object.");
    }

    o->type = oNone;
    o->iRefCount = 1;
    o->pCell = NULL;
    return o;
}

void object_freeObject(Object *o)
{
    if (o != NULL) {
        assert(o->iRefCount > 0);
        o->iRefCount--;

        if (o->iRefCount <= 0) {
            if (o->pCell != NULL) {
                cell_freeCell(o->pCell);
            }
            free(o);
        }
    }
}


Object *object_createArrayObject(Array *a)
{
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate Object container for array object.");
    }

    o->type = oArray;
    o->pCell = cell_newCell();
    o->pCell->array = array_copyArray(a);
    o->pCell->type = cArray;
    o->iRefCount = 1;

    return o;
}


Object *object_createBooleanObject(BOOL b)
{
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate Object container for boolean object.");
    }

    o->type = oBoolean;
    o->pCell = cell_fromBoolean(b);
    o->iRefCount = 1;

    return o;
}

Object *object_createDictionaryObject(Dictionary *d)
{
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate Object container for dictionary object.");
    }

    o->type = oDictionary;
    o->pCell = cell_fromDictionary(d);
    o->iRefCount = 1;

    return o;
}

Object *object_createNumberObject(Number val)
{
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate Object container for number object.");
    }

    o->type = oNumber;
    o->pCell = cell_fromNumber(val);
    o->iRefCount = 1;

    return o;
}

Object * object_createPointerObject(void *p)
{
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate Object container for pointer object.");
    }

    o->type = oPointer;
    o->pCell = cell_fromPointer(p);
    o->iRefCount = 1;

    return o;
}

Object *object_createStringObject(TString *s)
{
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate Object container for string object.");
    }

    o->type = oString;
    o->pCell = cell_fromString(s);
    o->iRefCount = 1;

    return o;
}

Object *object_fromCell(Cell *c)
{
    // Construct an Object from an existing Cell pointer.  There is no need to create a copy of the cell
    // since it is reference counted.
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate Object container for cell object type %d.", c->type);
    }
    if (c->type == cArray) {
        o->type = oArray;
    } else if (c->type == cBoolean) {
        o->type = oBoolean;
    //} else if (c->type == cBytes) {  // ToDo: Implement ObjectBytes
    //    o->type = oBytes;
    } else if (c->type == cDictionary) {
        o->type = oDictionary;
    } else if (c->type == cNumber) {
        o->type = oNumber;
    } else if (c->type == cPointer) {
        o->type = oPointer;
    } else if (c->type == cString) {
        o->type = oString;
    }
    o->pCell = c;
    o->iRefCount = 1;

    return o;
}
