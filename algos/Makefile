CC = g++
CFLAGS = -Wall -ggdb
EXE = job
CPP = main.o Job.o JobDL.o JobList.o JobElem.o

all : $(EXE)

$(EXE) : $(CPP)
	$(CC) $(CFLAGS) -o $@ $^

%.o : %.c
	$(CC) $(CFLAGS) -c $^

clean:
	rm -f *.o $(EXE) *~

re: clean all