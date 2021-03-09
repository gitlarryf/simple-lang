using System;
using System.Collections.Generic;
using System.IO;

namespace csnex.rtl
{
    public partial class file
    {
        private Executor Exec;
        public file(Executor exe)
        {
            Exec = exe;
        }

        public void _CONSTANT_Separator()
        {
            Exec.stack.Push(new Cell(Path.PathSeparator.ToString()));
        }

        private void handle_error(Exception error, string path)
        {
            try {
                throw error.InnerException;
            } catch (UnauthorizedAccessException) {
                Exec.Raise("FileException.PermissionDenied", path);
            } catch (DirectoryNotFoundException) {
                Exec.Raise("FileException.PathNotFound", path);
            } catch (FileNotFoundException) {
                Exec.Raise("FileException.PathNotFound", path);
            } catch (IOException) {
                Exec.Raise("FileException.Exists", path);
            } catch (Exception ex) {
                Exec.Raise("FileException", string.Format("{0}: {1}", path, ex.HResult));
            }
        }
            //switch (typeof(error)) {
            //if (error.GetType() == System.UnauthorizedAccessException) {
            //    //The caller does not have the required permission.
            //    //case ArgumentException:;  // sourceFileName or destFileName is a zero-length string, contains only white space, or contains one or more invalid characters as defined by InvalidPathChars.
            //                                //sourceFileName or destFileName specifies a directory.
            //    //case ArgumentNullException:; // sourceFileName or destFileName is null.
            //    //case PathTooLongException:; // The specified path, file name, or both exceed the system-defined maximum length.
            //    case DirectoryNotFoundException:; // The path specified in sourceFileName or destFileName is invalid (for example, it is on an unmapped drive).
            //    case FileNotFoundException:; // sourceFileName was not found.
            //    case IOException:; // destFileName exists. -or- An I/O error has occurred
            //    case NotSupportedException:; // 

            //switch (error) {
            //    //case ERROR_ALREADY_EXISTS:      exec->rtl_raise(exec, "FileException.DirectoryExists", path);     break;
            //    //case ERROR_ACCESS_DENIED:       exec->rtl_raise(exec, "FileException.PermissionDenied", path);    break;
            //    //case ERROR_PATH_NOT_FOUND:      exec->rtl_raise(exec, "FileException.PathNotFound", path);        break;
            //    case ERROR_FILE_EXISTS:         exec->rtl_raise(exec, "FileException.Exists", path);              break;
            //    //case ERROR_PRIVILEGE_NOT_HELD:  exec->rtl_raise(exec, "FileException.PermissionDenied", path);    break;
            //    default:
            //    {
            //        char err[MAX_PATH + 32];
            //        snprintf(err, sizeof(err), "%s: %d", path, error);
            //        exec->rtl_raise(exec, "FileException", err);
            //        break;
            //    }
            //}
        //}

        //private Number unix_time_from_filetime(FILETIME *ft)
        //{
        //    return number_divide(
        //                        number_from_uint64((((uint64_t)(ft->dwHighDateTime) << 32) | ft->dwLowDateTime) - 116444736000000000ULL),
        //                        number_from_uint32(10000000)
        //        );
        //}

        public void readBytes()
        {
            string filename = Exec.stack.Pop().String;

            FileStream f;
            try {
                f = new FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            } catch {
                Exec.Raise("FileException.Open", filename);
                return;
            }

            Cell r = new Cell(Cell.Type.Bytes);
            for (;;) {
                byte []buf = new byte[16384];
                int n = f.Read(buf, 0, 16384);
                if (n == 0) {
                    break;
                }
                Array.Copy(buf, 0, r.Bytes, r.Bytes.Length, n);
            }

            Exec.stack.Push(r);
        }

        public void readLines()
        {
            string filename = Exec.stack.Pop().String;

            FileStream f;
            try {
                f = new FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            } catch {
                Exec.Raise("FileException.Open", filename);
                return;
            }

            Cell r = new Cell(Cell.Type.Array);
            StreamReader sr = new StreamReader(f);
            for (;;) {
                if (sr.EndOfStream) {
                    break;
                }
                r.Array.Add(new Cell(sr.ReadLine()));
            }
            f.Close();

            Exec.stack.Push(r);
        }

        public void writeBytes()
        {
            Cell bytes = Exec.stack.Pop();
            string filename = Exec.stack.Pop().String;

            FileStream f;
            try {
                f = new FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            } catch {
                Exec.Raise("FileException.Open", filename);
                return;
            }

            try {
                f.Write(bytes.Bytes, 0, bytes.Bytes.Length);
            } catch {
                Exec.Raise("FileException.Write", filename);
                f.Close();
                return;
            }
            f.Close();

            Exec.stack.Push(bytes);
        }

        public void writeLines()
        {
            Cell lines = Exec.stack.Pop();
            string filename = Exec.stack.Pop().String;

            FileStream f;
            try {
                f = new FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Write, System.IO.FileShare.Read);
            } catch {
                Exec.Raise("FileException.Open", filename);
                return;
            }

            for (int i = 0; i < lines.Array.Count; i++) {
                try {
                    StreamWriter sw = new StreamWriter(f);
                    sw.WriteLine(lines.Array[i].String);
                } catch {
                    Exec.Raise("FileException.Write", filename);
                }
            }
            f.Close();
        }

        public void copy()
        {
            string destination = Exec.stack.Pop().String;
            string filename = Exec.stack.Pop().String;

            try {
                File.Copy(filename, destination, true);
            } catch (Exception ex) {
                handle_error(ex.InnerException, destination);
            }
            //} catch (UnauthorizedAccessException) {
            //    Exec.Raise("FileException.PermissionDenied", destination);
            //} catch (DirectoryNotFoundException) {
            //    Exec.Raise("FileException.PathNotFound", destination);
            //} catch (FileNotFoundException) {
            //    Exec.Raise("FileException.PathNotFound", destination);
            //} catch (IOException) {
            //    Exec.Raise("FileException.Exists", destination);
            //} catch (Exception ex) {
            //    Exec.Raise("FileException", string.Format("{0}: {1}", destination, ex.HResult));
            //}
        }

        public void copyOverwriteIfExists()
        {
            string destination = Exec.stack.Pop().String;
            string filename = Exec.stack.Pop().String;

            try {
                File.Copy(filename, destination, false);
            } catch (Exception ex) {
                handle_error(ex.InnerException, destination);
            }
        }

        public void delete()
        {
            string filename = Exec.stack.Pop().String;

            try {
                File.Delete(filename);
            } catch (Exception ex) {
                handle_error(ex, filename);
            }
        }

        public void exists()
        {
            string filename = Exec.stack.Pop().String;

            Exec.stack.Push(new Cell(File.Exists(filename)));
        }

        public void files()
        {
            string path = Exec.stack.Pop().String;

            Cell r = new Cell(Cell.Type.Array);
            path += Path.PathSeparator + "*";

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            List<FileInfo> files = new List<FileInfo>(dirInfo.GetFiles("*", SearchOption.TopDirectoryOnly));
            foreach (FileInfo f in files) {
                r.Array.Add(new Cell(f.Name));
            }

            Exec.stack.Push(r);
        }

        public void getInfo()
        {
            string name = Exec.stack.Pop().String;

            FileInfo fi;
            try {
                fi = new FileInfo(name);
            } catch (Exception ex) {
                handle_error(ex, name);
                return;
            }

            Cell r = new Cell(Cell.Type.Array);
            // name: String
            r.Array.Add(new Cell(fi.Name));
            // size: Number
            r.Array.Add(new Cell(new Number(fi.Length)));
            // readable: Boolean
            r.Array.Add(new Cell(true));
            // writable: Boolean
            r.Array.Add(new Cell(fi.IsReadOnly == false));
            // executable: Boolean
            r.Array.Add(new Cell(false));
            // type: FileType
            Number n = new Number((int)((fi.Attributes & FileAttributes.Directory) == FileAttributes.Directory ? FileType.directory :
                                        (fi.Attributes & FileAttributes.Device) == FileAttributes.Device ? FileType.special :
                                        (fi.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint ? FileType.special :
                                        FileType.normal));
            r.Array.Add(new Cell(n));
            // creationTime: Number
            r.Array.Add(new Cell(new Number(fi.CreationTime.ToFileTime())));
            // lastAccessTime: Number
            r.Array.Add(new Cell(new Number(fi.LastAccessTime.ToFileTime())));
            // lastModificationTime: Number
            r.Array.Add(new Cell(new Number(fi.LastWriteTime.ToFileTime())));

            Exec.stack.Push(r);
        }

        public void isDirectory()
        {
            string path = Exec.stack.Pop().String;

            FileInfo attr = new FileInfo(path);
            Exec.stack.Push(new Cell((attr.Attributes & FileAttributes.Directory) == FileAttributes.Directory));
        }

        public void mkdir()
        {
            string path = Exec.stack.Pop().String;

            try {
                Directory.CreateDirectory(path);
            } catch (Exception ex) {
                handle_error(ex, path);
            }
        }

        public void removeEmptyDirectory()
        {
            string path = Exec.stack.Pop().String;

            try {
                Directory.Delete(path, false);
            } catch (Exception ex) {
                handle_error(ex, path);
            }
        }

        public void rename()
        {
            string newname = Exec.stack.Pop().String;
            string oldname = Exec.stack.Pop().String;

            try {
                File.Move(oldname, newname);
            } catch (Exception ex) {
                handle_error(ex, oldname);
            }
        }
    }
}
