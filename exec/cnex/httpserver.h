#ifndef HTTPSERVER_H
#define HTTPSERVER_H
#include "lib/socketx.h"

typedef struct THttpResponse {
    unsigned int code;
    struct tagTString *content;
    struct tagTString*(*to_string)(struct THttpResponse*);
} HttpResponse;

struct TExecutor;

typedef struct IHttpServerHandler {
    void(*handle_GET)(/*struct tagTExecutor *exec,*/ struct tagTString *path, struct THttpResponse *response);
    void(*handle_POST)(/*struct tagTExecutor *exec,*/ struct tagTString *path, struct tagTString *data, struct THttpResponse *response);
} HttpServerHandler;

typedef struct TClient {
    struct IHttpServerHandler *handler;
    SOCKET socket;
    struct tagTString *request;
    struct tagTString *path;
    int post_length;
    struct tagTString *post_data;
    struct tagTString *content_length;
    struct TClient *next;
} Client;

//typedef struct TClients {
//    struct TClient *client;
//    struct TClient *next;
//} Clients;

typedef struct THttpServer {
    struct IHttpServerHandler *handler;
    SOCKET server;
    struct TClient *clients;
    struct TExecutor *exec;
    void(*service)(BOOL wait);
    void(*handle_request)(struct TClient *client);
} HttpServer;

struct tagTString *http_responseToString(struct THttpResponse *r);


struct THttpResponse *response_newHttpResponse();
struct tagTString *http_responseToString(struct THttpResponse *r);

BOOL client_headerMatch(struct tagTString *header, struct tagTString *target, struct tagTString *value);
BOOL client_handleData(Client *c, char *buff, int n);
void client_handleResponse(Client *c, HttpResponse *response);
BOOL client_handleRequest(Client *c);
Client *client_newClient(SOCKET s);
void client_freeClient(Client *c);

HttpServer *http_initServer(unsigned short port, HttpServerHandler *handler);
void http_serverRemoveClient(HttpServer *server, Client *d);
void http_serverAddClient(HttpServer *server, SOCKET client);
void http_serverService(HttpServer *server, BOOL wait);
void http_serverShutdown(HttpServer *server);

#endif
