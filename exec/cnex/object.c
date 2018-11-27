#include <stdint.h>
#include <string.h>

#include "array.h"
#include "cell.h"
#include "dictionary.h"
#include "number.h"
#include "nstring.h"
#include "util.h"

#include "object.h"

TString *object_toString(Object *obj)
{
    if (obj->type == oArray) {
        TString *r = string_createString(0);
        size_t x;
        Array *a = &((ObjectArray*)obj->pObject)->array;
        r = string_appendCString(r, "[");
        for (x = 0; x < a->size; x++) {
            if (r->length > 1) {
                r = string_appendCString(r, ", ");
            }
            r = string_appendString(r, cell_toString(&a->data[x]));
        }
        r = string_appendCString(r, "]");
        return r;
    } else if (obj->type == oBoolean) {
        if (((ObjectBoolean*)obj->pObject)->boolean == TRUE) {
            return string_createCString("TRUE");
        }
        return string_createCString("FALSE");
    } else if(obj->type == oBytes) {
        BOOL first = TRUE;
        TString *r = string_createCString("HEXBYTES \"");
        TString *bytes = &((ObjectString*)obj->pObject)->string;
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
        return r;
    } else if(obj->type == oDictionary) {
        TString *r = string_createString(0);
        size_t x;
        Dictionary *d = &((ObjectDictionary*)obj->pObject)->dictionary;
        r = string_appendCString(r, "{");
        Cell *keys = dictionary_getKeys(d);
        for (x = 0; x < keys->array->size; x++) {
            if (r->length > 1) {
                r = string_appendCString(r, ", ");
            }
            r = string_appendCString(r, "\"");
            r = string_appendString(r, keys->array->data[x].string);
            r = string_appendCString(r, "\": ");
            r = string_appendString(r, cell_toString(dictionary_findDictionaryEntry(d, keys->array->data[x].string)));
        }
        r = string_appendCString(r, "}");
        return r;
    } else if(obj->type == oNumber) {
        return string_createCString(number_to_string(((ObjectNumber*)obj->pObject)->number));
    } else if(obj->type == oString) {
        TString *r = string_createCString("\"");
        r = string_appendString(r, &((ObjectString*)obj->pObject)->string);
        r = string_appendCString(r, "\"");
        return r;
    }
    return string_createCString("Invalid Type Conversion");
}

Object *object_createObject()
{
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate object.");
    }

    o->type = oNone;
    o->pObject = NULL;
    return o;
}

void object_freeObject(Object *o)
{
    if (o != NULL) {
        if (o->pObject != NULL) {
            free(o->pObject);
        }
        free (o);
    }
}

Object *object_copyObject(Object *o)
{
    Object *r = object_createObject();

    switch (o->type) {
        case oArray:
            r->pObject = malloc(sizeof(ObjectArray));
            if (r->pObject == NULL) {
                fatal_error("Could not create copy of array object.");
            }
            r->type = o->type;
            memcpy(r->pObject, o->pObject, sizeof(ObjectArray));
            break;
        case oBoolean:
            r->pObject = malloc(sizeof(ObjectBoolean));
            if (r->pObject == NULL) {
                fatal_error("Could not create copy of boolean object.");
            }
            r->type = o->type;
            memcpy(r->pObject, o->pObject, sizeof(ObjectBoolean));
            break;
        case oDictionary:
            r->pObject = malloc(sizeof(ObjectDictionary));
            if (r->pObject == NULL) {
                fatal_error("Could not create copy of dictionary object.");
            }
            r->type = o->type;
            r->type = o->type;
            Dictionary *pDic = &((ObjectDictionary*)o->pObject)->dictionary;
            Dictionary *pr = &((ObjectDictionary*)r->pObject)->dictionary;
            pr->len = 0;
            pr->max = 8;
            pr->data = malloc(pr->max * sizeof(DictionaryEntry));
            for (int64_t i = 0; i < pDic->len; i++) {
                dictionary_addDictionaryEntry(pr, string_copyString(pDic->data[i].key), cell_fromCell(pDic->data[i].value));
            }
            break;
        case oNumber:
            r->pObject = malloc(sizeof(ObjectNumber));
            if (r->pObject == NULL) {
                fatal_error("Could not create copy of number object.");
            }
            r->type = o->type;
            memcpy(r->pObject, o->pObject, sizeof(ObjectNumber));
            break;
        case oPointer:
            r->pObject = malloc(sizeof(ObjectPointer));
            if (r->pObject == NULL) {
                fatal_error("Could not create copy of pointer objet.");
            }
            r->type = o->type;
            memcpy(r->pObject, o->pObject, sizeof(ObjectPointer));
            break;
        case oString:
            r->pObject = malloc(sizeof(ObjectString));
            if (r->pObject == NULL) {
                fatal_error("Could not create copy of string object.");
            }
            r->type = o->type;
            memcpy(r->pObject, o->pObject, sizeof(ObjectString));
            break;
        case oNone:
            r->pObject = NULL;
            r->type = oNone;
            break;
        default:
            fatal_error("Unknown object type: %d\n", o->type);
            break;
    }

    return r;
}

Object *object_createArrayObject(Array *a)
{
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate Object container for array object.");
    }

    o->pObject = malloc(sizeof(ObjectArray));
    if (o->pObject == NULL) {
        fatal_error("Could not create object array object.");
    }

    o->type = oArray;
    ((ObjectArray*)o->pObject)->type = o->type;
    ((ObjectArray*)o->pObject)->array.size = 0;
    ((ObjectArray*)o->pObject)->array.data = NULL;
    Array *pArr = &((ObjectArray*)o->pObject)->array;

    for (size_t i = 0; i < a->size; i++) {
        array_addArrayElement(pArr, &a->data[i]);
    }
    return o;
}


Object *object_createBooleanObject(BOOL b)
{
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate Object container for boolean object.");
    }

    o->pObject = malloc(sizeof(ObjectBoolean));
    if (o->pObject == NULL) {
        fatal_error("Could not create object boolean object.");
    }

    o->type = oBoolean;
    ((ObjectBoolean*)o->pObject)->type = o->type;
    ((ObjectBoolean*)o->pObject)->boolean = b;

    return o;
}

Object *object_createDictionaryObject(Dictionary *d)
{
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate Object container for dictionary object.");
    }

    o->pObject = malloc(sizeof(ObjectDictionary));
    if (o->pObject == NULL) {
        fatal_error("Could not create object dictionary object.");
    }

    o->type = oDictionary;
    ((ObjectDictionary*)o->pObject)->type = o->type;
    Dictionary *pDic = &((ObjectDictionary*)o->pObject)->dictionary;
    pDic->len = 0;
    pDic->max = 8;
    pDic->data = malloc(pDic->max * sizeof(DictionaryEntry));
    for (int64_t i = 0; i < d->len; i++) {
        dictionary_addDictionaryEntry(pDic, string_copyString(d->data[i].key), cell_fromCell(d->data[i].value));
    }

    return o;
}

Object *object_createNumberObject(Number val)
{
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate Object container for number object.");
    }

    o->pObject = malloc(sizeof(ObjectNumber));
    if (o->pObject == NULL) {
        fatal_error("Could not create object number object.");
    }

    o->type = oNumber;
    ((ObjectNumber*)o->pObject)->type = oNumber;
    ((ObjectNumber*)o->pObject)->number = val;

    return o;
}

Object * object_createPointerObject(void * p)
{
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate Object container for pointer object.");
    }

    o->pObject = malloc(sizeof(ObjectPointer));
    if (o->pObject == NULL) {
        fatal_error("Could not create object pointer object.");
    }

    o->type = oPointer;
    ((ObjectPointer*)o->pObject)->type = oPointer;
    ((ObjectPointer*)o->pObject)->pointer = p;

    return o;
}

Object *object_createStringObject(TString *s)
{
    Object *o = malloc(sizeof(Object));
    if (o == NULL) {
        fatal_error("failed to allocate Object container for string object.");
    }

    o->pObject = malloc(sizeof(ObjectString));
    if (o->pObject == NULL) {
        fatal_error("Could not create object string object.");
    }

    o->type = oString;

    ((ObjectString*)o->pObject)->type = oString;
    ((ObjectString*)o->pObject)->string.length = s->length;
    ((ObjectString*)o->pObject)->string.data = malloc(s->length);
    if (((ObjectString*)o->pObject)->string.data == NULL) {
        fatal_error("Could not allocate storage for string object. (len=%d)", s->length);
    }
    memcpy(((ObjectString*)o->pObject)->string.data, s->data, s->length);

    return o;
}

Number *object_getNumberObject(Object *o)
{
    if (((ObjectNumber*)o->pObject)->type == oNumber) {
        return &((ObjectNumber*)o->pObject)->number;
    }
    return NULL;
}

