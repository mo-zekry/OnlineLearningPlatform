// function initPage(endpoint) {
//   document.addEventListener("DOMContentLoaded", function () {
//     document
//       .getElementById("search-query")
//       .addEventListener("input", function () {
//         var query = this.value.trim();
//         if (query.length > 0) {
//           document.getElementById("category-filter").value = "";
//           fetchSearchResults(endpoint, query);
//         } else {
//           fetchAllCourses(endpoint);
//         }
//       });

//     document
//       .getElementById("category-filter")
//       .addEventListener("change", function () {
//         var id = this.value.trim();
//         if (id.length > 0) {
//           document.getElementById("search-query").value = "";
//           fetchFilteredResults(endpoint, id);
//         } else {
//           fetchAllCourses(endpoint);
//         }
//       });

//     document
//       .getElementById("search-query")
//       .addEventListener("keydown", function (event) {
//         if (event.key === "Enter") {
//           event.preventDefault();
//           document.getElementById("search-button").click();
//         }
//       });

//     document
//       .getElementById("search-button")
//       .addEventListener("click", function () {
//         var query = document.getElementById("search-query").value.trim();
//         if (query.length > 0) {
//           fetchSearchResults(endpoint, query);
//         }
//       });

//     function fetchSearchResults(endpoint, query) {
//       fetch(`/${endpoint}/Search?query=${query}`, {
//         method: "POST",
//         headers: {
//           "Content-Type": "application/json",
//         },
//         body: JSON.stringify({ query: query }),
//       })
//         .then((response) => response.text())
//         .then((result) => {
//           document.getElementById("course-list").innerHTML = result;
//         })
//         .catch((error) => {
//           alert("Error loading search results");
//         });
//     }

//     function fetchFilteredResults(endpoint, id) {
//       fetch(`/${endpoint}/Filter/${id}`, {
//         method: "POST",
//         headers: {
//           "Content-Type": "application/json",
//         },
//         body: JSON.stringify({ categoryId: id }),
//       })
//         .then((response) => response.text())
//         .then((result) => {
//           var courseList = document.getElementById("course-list");
//           while (courseList.hasChildNodes()) {
//             courseList.removeChild(courseList.firstChild);
//           }
//           courseList.innerHTML = result;
//         })
//         .catch((error) => {
//           alert("Error loading filtered results");
//         });
//     }

//     function fetchAllCourses(endpoint) {
//       fetch(`/${endpoint}/GetCourses`, {
//         method: "POST",
//         headers: {
//           "Content-Type": "application/json",
//         },
//         body: JSON.stringify({}),
//       })
//         .then((response) => response.text())
//         .then((result) => {
//           document.getElementById("course-list").innerHTML = result;
//         })
//         .catch((error) => {
//           alert("Error loading courses");
//         });
//     }
//   });
// }


// export default initPage;