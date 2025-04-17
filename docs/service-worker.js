const cacheName = 'disney-checklist-cache-v1';
const filesToCache = [
  './',
  './index.html',
  './Build/DisneyChecklist.loader.js',
  './Build/DisneyChecklist.framework.js',
  './Build/DisneyChecklist.wasm',
  './Build/DisneyChecklist.data',
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
  console.log("Service worker activated.");
});

self.addEventListener("fetch", function (e) {
  e.respondWith(
    caches.match(e.request).then(response =>
      response || fetch(e.request)
    )
  );
});