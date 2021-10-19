package logger

import (
	"bytes"
	"go.uber.org/zap/zapcore"
	"log"
	"net/http"
	"sync"
	"time"
)

const timeout = 15 * time.Second

type FluentBit struct {
	url       string
	client    *http.Client
	queue     chan []byte
	waitGroup sync.WaitGroup
}

func NewFluentBit(url string) *FluentBit {
	fb := &FluentBit{
		url:    url,
		client: &http.Client{Timeout: timeout},
		queue:  make(chan []byte),
	}

	go fb.run()

	return fb
}

func (f *FluentBit) run() {
	f.waitGroup.Add(1)
	for {
		f.waitGroup.Done()

		data, ok := <-f.queue
		if !ok {
			break
		}

		log.Print(string(data))
		resp, err := f.client.Post(f.url, "application/json", bytes.NewReader(data))
		if err != nil {
			log.Println(err)
			continue
		}

		if resp.StatusCode != http.StatusCreated {
			log.Printf("invalid status %d %s", resp.StatusCode, resp.Status)
		}
	}
}

func (f *FluentBit) Write(p []byte) (n int, err error) {
	f.queue <- p
	f.waitGroup.Add(1)
	return len(p), nil
}

func (f *FluentBit) Sync() error {
	f.waitGroup.Wait()
	close(f.queue)
	return nil
}

func (f *FluentBit) Lock() zapcore.WriteSyncer {
	return zapcore.Lock(f)
}
