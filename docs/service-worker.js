self.addEventListener("install", function (e) {
    console.log("Service worker installed.");
    self.skipWaiting();
  });
  
  self.addEventListener("activate", function (e) {
    console.log("Service worker activated.");
  });
  
  self.addEventListener("fetch", function (e) {
    
  });
