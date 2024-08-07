document.addEventListener("DOMContentLoaded", function () {
    document
        .getElementById("search-query")
        .addEventListener("input", function () {
            var query = this.value.trim();
            if (query.length > 0) {
                document.getElementById("category-filter").value = "";
                fetchSearchResults(query);
            } else {
                fetchAllCourses();
            }
        });

    document
        .getElementById("category-filter")
        .addEventListener("change", function () {
            var id = this.value.trim();
            if (id.length > 0) {
                document.getElementById("search-query").value = "";
                fetchFilteredResults(id);
            } else {
                fetchAllCourses();
            }
        });

    document
        .getElementById("search-query")
        .addEventListener("keydown", function (event) {
            if (event.key === "Enter") {
                event.preventDefault();
                document.getElementById("search-button").click();
            }
        });

    // => if add a search button
    //   document
    //     .getElementById("search-button")
    //     .addEventListener("click", function () {
    //       var query = document.getElementById("search-query").value.trim();
    //       if (query.length > 0) {
    //         fetchSearchResults(query);
    //       }
    //     });

    function fetchSearchResults(query) {
        fetch(`/Course/Search?query=${query}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({query: query}),
        })
            .then((response) => response.text())
            .then((result) => {
                document.getElementById("course-list").innerHTML = result;
            })
            .catch((error) => {
                alert("Error loading search results");
            });
    }

    function fetchFilteredResults(id) {
        fetch(`/Course/Filter/${id}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({categoryId: id}),
        })
            .then((response) => response.text())
            .then((result) => {
                var courseList = document.getElementById("course-list");
                while (courseList.hasChildNodes()) {
                    courseList.removeChild(courseList.firstChild);
                }
                courseList.innerHTML = result;
            })
            .catch((error) => {
                alert("Error loading filtered results");
            });
    }

    function fetchAllCourses() {
        fetch(`/Course/GetCourses`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({}),
        })
            .then((response) => response.text())
            .then((result) => {
                document.getElementById("course-list").innerHTML = result;
            })
            .catch((error) => {
                alert("Error loading courses");
            });
    }
});

