class Mutex {
  constructor() {
    this.queue = [];
    this.locked = false;
  }

  async lock() {
    return new Promise((resolve) => {
      this.queue.push(resolve);
      this._checkQueue();
    });
  }

  release() {
    this.locked = false;
    this._checkQueue();
  }

  _checkQueue() {
    if (this.locked || this.queue.length === 0) return;

    const resolve = this.queue.shift();
    this.locked = true;
    resolve(() => this.release());
  }
}

export default Mutex;
