package logger

import (
	"bytes"
	"go.uber.org/zap/zapcore"
	"log"
	"net/http"
	"sync"
	"time"
)

const DefaultTimeout = 15 * time.Second

type FluentBit struct {
	url       string
	client    *http.Client
	queue     chan []byte
	waitGroup sync.WaitGroup
}

func NewFluentBit(url string, timeout time.Duration) *FluentBit {
	fb := &FluentBit{
		url:    url,
		client: &http.Client{Timeout: timeout},
		queue:  make(chan []byte),
	}

	go fb.run()

	return fb
}

func (f *FluentBit) run() {
	for {
		data, ok := <-f.queue
		if !ok {
			break
		}

		f.waitGroup.Done()
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
	f.waitGroup.Add(1)
	f.queue <- p
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
