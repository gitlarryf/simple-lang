#include "httpserver.h"

#include <assert.h>

#include "nstring.h"

#include "lib/socketx.h"

const int POST_MAX_LENGTH = 10000000;

static Client *http_clients;


HttpResponse *response_newHttpResponse()
{
    HttpResponse *r = malloc(sizeof(HttpResponse));
    r->code = 0;
    r->content = NULL;
    r->to_string  = http_responseToString;
}

TString *http_responseToString(HttpResponse *r)
{
    char data[64];
    snprintf(data, sizeof(data), "HTTP/1.0 %d ok\r\nContent-length: %llu\r\n\r\n", r->code, r->content->length);
    TString *s = string_newString();
    string_appendCString(s, data);
    string_appendData(s, r->content->data, r->content->length);
    return s;
}

BOOL client_headerMatch(TString *header, TString *target, TString *value)
{
    TString *h = string_toLowerCase(string_subString(header, 0, target->length));
    TString *t = string_toLowerCase(target);

    if (string_compareString(h, t) == 0) {
        size_t i = target->length;
        while (i < header->length && isspace(header->data[i])) {
            i++;
        }
        value = string_subString(header, i, header->length - i);
        return TRUE;
    }
    return FALSE;
}

//bool Client::header_match(const std::string &header, const std::string &target, std::string &value)
//{
//    std::string h = header.substr(0, target.length());
//    std::string t = target;
//    //std::transform(h.begin(), h.end(), h.begin(), tolower);
//    //std::transform(t.begin(), t.end(), t.begin(), tolower);
//    for (auto &c: h) {
//        c = static_cast<char>(::tolower(c));
//    }
//    for (auto &c: t) {
//        c = static_cast<char>(::tolower(c));
//    }
//    if (h == t) {
//        auto i = target.length();
//        while (i < header.length() && isspace(header[i])) {
//            ++i;
//        }
//        value = header.substr(i);
//        return true;
//    } else {
//        return false;
//    }
//}

//class Client {
//public:
//    Client(IHttpServerHandler *handler, SOCKET socket): handler(handler), socket(socket), request(), path(), post_length(-1), post_data() {}
//    Client(const Client &) = delete;
//    Client &operator=(const Client &) = delete;
//    ~Client();
//    IHttpServerHandler *handler;
//    SOCKET socket;
//    std::string request;
//    std::string path;
//    int post_length;
//    std::string post_data;
//
//    bool handle_data(const std::string &data);
//    bool handle_request();
//    void handle_response(HttpResponse &response);
//    static bool header_match(const std::string &header, const std::string &target, std::string &value);
//};

BOOL client_handleData(Client *c, char *buff, int n)
{
    //TString *data = string_createCString(buff);
    if (c->post_length < 0) {
        string_appendData(c->request, buff, n);
        int64_t end = string_findCString(c->request, 0, "\r\n\r\n");
        //auto end = request.find("\r\n\r\n");
        if (end != NPOS) {
            end += 4;
            c->post_data = string_subString(c->request, end, c->request->length - end);
            c->request = string_subString(c->request, 0, end);
            if (client_handleRequest(c)) {
                return TRUE;
            }
        }
    } else {
        c->post_data = string_appendData(c->post_data, buff, n);
        c->post_length -= (int)n;
        if (c->post_length <= 0) {
            HttpResponse *response = response_newHttpResponse();
            c->handler->handle_POST(c->path, c->post_data, response);
            client_handleResponse(c, response);
            return TRUE;
        }
    }
    return FALSE;
}

void client_handleResponse(Client *c, HttpResponse *response)
{
    TString *r = http_responseToString(response);
    send(c->socket, r->data, (int)r->length, 0);
    string_freeString(r);
}

BOOL client_handleRequest(Client *c)
{
    StringArray *lines = stringarray_createArray();
    size_t i = 0;
    for (;;) {
        size_t start = i;
        i = string_findCString(c->request, i, "\r\n");
        if (i == NPOS || i == start) {
            break;
        }
        stringarray_addString(lines, string_subString(c->request, start, i));
        i += 2;
    }
    //StringArray *req = stringarray_createArray();
    StringArray *req = stringarray_splitString(lines->data[0], ' ');
    TString *method = req->data[0];
    string_copyString(c->path, req->data[1]);
    //HttpResponse *response = malloc(sizeof(HttpResponse));
    HttpResponse *response = response_newHttpResponse();
    if (string_compareCString(method, "GET") == 0) {
        c->handler->handle_GET(c->path, response);
    } else if(string_compareCString(method, "POST") == 0) {
        for (size_t l = 0; l < lines->size; l++) {
            TString *value = string_newString();
            if (client_headerMatch(lines->data[l], c->content_length, value)) {
                int length = atoi(value->data);
                if (length >= 0 && length <= POST_MAX_LENGTH) {
                    c->post_length = length;
                    c->post_length -= (int)c->post_data->length;
                    if (c->post_length > 0) {
                        return FALSE;
                    } else {
                        c->handler->handle_POST(c->path, c->post_data, response);
                    }
                }
                break;
            }
        }
    } else {
        response->code = 404;
    }
    client_handleResponse(c, response);
    return TRUE;
}
//bool Client::handle_request()
//{
//    std::vector<std::string> lines;
//    std::string::size_type i = 0;
//    for (;;) {
//        auto start = i;
//        i = request.find("\r\n", i);
//        if (i == std::string::npos || i == start) {
//            break;
//        }
//        lines.push_back(request.substr(start, i));
//        i += 2;
//    }
//    std::vector<std::string> req = split(lines[0], ' ');
//    const std::string &method = req[0];
//    path = req[1];
//    //const std::string &version = req[2];
//    HttpResponse response;
//    if (method == "GET") {
//        handler->handle_GET(path, response);
//    } else if (method == "POST") {
//        for (auto &h: lines) {
//            std::string value;
//            if (header_match(h, "Content-Length:", value)) {
//                int length = std::stoi(value);
//                if (length >= 0 && length <= POST_MAX_LENGTH) {
//                    post_length = length;
//                    post_length -= static_cast<int>(post_data.length());
//                    if (post_length > 0) {
//                        return false;
//                    } else {
//                        handler->handle_POST(path, post_data, response);
//                    }
//                }
//                break;
//            }
//        }
//    } else {
//        response.code = 404;
//    }
//    handle_response(response);
//    return true;
//}


//class HttpServerImpl {
//public:
//    HttpServerImpl(unsigned short port, IHttpServerHandler *handler);
//    HttpServerImpl(const HttpServerImpl &) = delete;
//    HttpServerImpl &operator=(const HttpServerImpl &) = delete;
//    ~HttpServerImpl();
//
//    IHttpServerHandler *const handler;
//    SOCKET server;
//    std::list<Client> clients;
//
//    void service(bool wait);
//    void handle_request(Client &client);
//};

HttpServer *http_initServer(unsigned short port, HttpServerHandler *handler)
{
    HttpServer *server = malloc(sizeof(HttpServer));
    if (server == NULL) {
        fatal_error("Could not allocate memory for HTTP Server.");
    }
    server->handler = handler;
    server->clients = NULL;
    server->service = NULL;
    server->handle_request = NULL;

#ifdef _WIN32
    static BOOL initialized = FALSE;
    if (!initialized) {
        WSADATA wsa;
        WSAStartup(MAKEWORD(1, 1), &wsa);
        initialized = TRUE;
    }
#endif
    server->server = socket(PF_INET, SOCK_STREAM, 0);
    assert(server->server != INVALID_SOCKET);
    int on = 1;
    int r = setsockopt(server->server, SOL_SOCKET, SO_REUSEADDR, (char *)(&on), sizeof(on));
    assert(r == 0);
    struct sockaddr_in sin;
    sin.sin_family = AF_INET;
    sin.sin_addr.s_addr = INADDR_ANY;
    sin.sin_port = htons(port);
    r = bind(server->server, (struct sockaddr *)(&sin), sizeof(sin));
    if (r != 0) {
        perror("debug server bind failure");
        closesocket(server->server);
        server->server = INVALID_SOCKET;
        return server;
    }
    r = listen(server->server, 5);
    assert(r == 0);
    return server;
}

void http_serverShutdown(HttpServer *server)
{
    if (server->server != INVALID_SOCKET) {
        closesocket(server->server);
    }
    free(server->handler);
    Client *c = server->clients;
    while (c) {
        Client *n = c->next;
        client_freeClient(c);
        c = n;
    }
    free(server);
}

Client *client_newClient(SOCKET socket)
{
    Client *r = malloc(sizeof(Client));
    if (r == NULL) {
        fatal_error("Could not allocate memory for new HTTP Client.");
    }

    r->next = NULL;
    r->socket = socket;
    r->handler = NULL;
    r->path = NULL;
    r->post_data = string_newString();
    r->post_length = -1;
    r->request = string_newString();

    r->content_length = string_createCString("Content-Length");
    return r;
}

void client_freeClient(Client *c)
{
    assert(c != NULL);
    string_freeString(c->path);
    string_freeString(c->post_data);
    string_freeString(c->request);
    string_freeString(c->content_length);
    free(c->handler);
    // Note: We will NOT be free()'ing c->next here!
    if (c->socket != INVALID_SOCKET) {
        closesocket(c->socket);
    }
    free(c);
}

void http_serverAddClient(HttpServer *server, SOCKET client)
{
    // Always start at the root...
    Client *c = server->clients;

    Client *nc = client_newClient(client);
    nc->handler = server->handler;

    // If we haven't initialized the connected clients yet, go ahead and set
    // client to the first client and return.
    if (c == NULL) {
        server->clients = nc;
        return;
    }

    // Jump to the end of the list; to add this client to the end of the list.
    while (c && c->next) {
        c = c->next;
    }

    // Add this new client to the list of clients.
    c->next = nc;
}

void http_serverRemoveClient(HttpServer *server, Client *d)
{
    Client *c = server->clients;

    if (d == server->clients && d->next != NULL) {
        // We need to remove the first client on the list.
        server->clients = d->next;
    } else if (d == server->clients && d->next == NULL) {
        // There are no more clients in the list, so just free the target client.
        server->clients = NULL;
    } else {
        // Iterate until we find the client in the list pointing to 
        // the client we want to free, then adjust the ->next member
        // to point to the targets next member.
        Client *prev = c;
        while (c) {
            if (c == d) {
                prev->next = d->next;
                break;
            }
            prev = c;
            c = c->next;
        }
    }
    client_freeClient(d);
}


void http_serverService(HttpServer *server, BOOL wait)
{
    if (server->server == INVALID_SOCKET) {
        return;
    }
    fd_set rfds;
    FD_ZERO(&rfds);
    FD_SET(server->server, &rfds);
    SOCKET maxsocket = server->server;
    Client *c = server->clients;
    while (c != NULL) {
        FD_SET(c->socket, &rfds);
        if (c->socket > maxsocket) {
            maxsocket = c->socket;
        }
        c = c->next;
    }
    struct timeval tv = { 0, 0 };
    int n = select((int)(maxsocket+1), &rfds, NULL, NULL, wait ? NULL : &tv);
    if (n > 0) {
        if (FD_ISSET(server->server, &rfds)) {
            struct sockaddr_in sin;
            socklen_t socklen = sizeof(sin);
            SOCKET t = accept(server->server, (struct sockaddr *)(&sin), &socklen);
            assert(t != INVALID_SOCKET);
            http_serverAddClient(server, t);
        }
        c = server->clients;
        while (c != NULL) {
            if (FD_ISSET(c->socket, &rfds)) {
                char buf[200];
                n = recv(c->socket, buf, sizeof(buf), 0);
                if (n > 0) {
                    if (client_handleData(c, buf, n)) {
                        Client *d = c;
                        c = d->next;
                        http_serverRemoveClient(server, d);
                    } else {
                        c = c->next;
                    }
                } else {
                    Client *d = c;
                    c = d->next;
                    http_serverRemoveClient(server, d);
                }
            } else {
                c = c->next;
            }
        }
    }
}

