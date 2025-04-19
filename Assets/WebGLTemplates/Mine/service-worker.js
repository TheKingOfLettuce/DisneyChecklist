const cacheName = 'disney-checklist-cache-v7';
const filesToCache = [
  './',
  './index.html',
  './Build/docs.loader.js',
  './Build/docs.framework.js',
  './Build/docs.wasm',
  './Build/docs.data',
  './img/D180.png',
  './img/512.png',
];

self.addEventListener("install", function (e) {
  e.waitUntil(
    caches.open(cacheName).then(cache => cache.addAll(filesToCache))
  );
  console.log("Service worker installed.");
});

self.addEventListener("activate", function (e) {
  e.waitUntil(
    caches.keys().then(keys =>
      Promise.all(
        keys.map(key => {
          if (key !== cacheName) return caches.delete(key);
        })
      )
    )
  );
});

self.addEventListener("fetch", function (e) {
  e.respondWith(
    caches.match(e.request).then(response =>
      response || fetch(e.request)
    )
  );
});