#ifndef OBJECT_H
#define OBJECT_H
#include <stdint.h>

#include "array.h"
#include "dictionary.h"
#include "number.h"
#include "nstring.h"
#include "util.h"

typedef enum tagNObjectType {
    oNone,
    oArray,
    oBoolean,
    oBytes,
    oDictionary,
    oNumber,
    oPointer,
    oString,
} ObjectType;

// ToDo: Switch all Object support over to a single Cell!
//typedef struct tagTObject {
//    ObjectType      type;
//    struct tagTCell *pObject;
//} Object;

typedef struct tagTObject {
    ObjectType  type;
    void        *pObject;
} Object;

typedef struct tagTObjectArray {
    enum tagNObjectType type;
    struct tagTArray array;
} ObjectArray;

typedef struct tagTObjectBoolean {
    enum tagNObjectType type;
    BOOL boolean;
} ObjectBoolean;

typedef struct tagTObjectBytes {
    enum tagNObjectType type;
    struct tagTString bytes;
} ObjectBytes;

typedef struct tagTObjectDictionary {
    enum tagNObjectType type;
    struct tagTDictionary dictionary;
} ObjectDictionary;

typedef struct tagTObjectNumber {
    enum tagNObjectType type;
    Number number;
} ObjectNumber;

typedef struct tagTObjectPointer {
    enum tagNObjectType type;
    void *pointer;
} ObjectPointer;

typedef struct tagTObjectString {
    enum tagNObjectType type;
    struct tagTString string;
} ObjectString;

Object *object_createObject(void);
void object_freeObject(Object *o);
Object *object_copyObject(Object *o);

TString *object_toString(Object *s);

Object *object_createArrayObject(Array *a);
Object *object_createBooleanObject(BOOL b);
Object *object_createDictionaryObject(Dictionary *d);
Object *object_createNumberObject(Number n);
Object *object_createPointerObject(void *p);
Object *object_createStringObject(TString *s);

#endif
