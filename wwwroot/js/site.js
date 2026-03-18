// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function addPlayer(targetDiv) {
    const container = document.getElementById(targetDiv);

    let index = container.querySelectorAll(".player-block").length;

    if (index >= 4) {
        alert("Teams must have exactly 4 players.");
        return;
    }

    container.innerHTML += `
        <div class="player-block border rounded p-3 mb-3">
            <h5>Player ${index + 1}</h5>

            <div class="mb-2">
                <label>Player Name</label>
                <input class="form-control" name="NewPlayers[${index}].PlayerName" />
            </div>

            <div class="mb-2">
                <label>City</label>
                <input class="form-control" name="NewPlayers[${index}].City" />
            </div>

            <div class="mb-2">
                <label>Province</label>
                <select class="form-select" name="NewPlayers[${index}].Province">
                    <option value="NB">New Brunswick</option>
                    <option value="NS">Nova Scotia</option>
                    <option value="ON">Ontario</option>
                </select>
            </div>

            <div class="mb-2">
                <label>Email</label>
                <input type="email" class="form-control" name="NewPlayers[${index}].Email" />
            </div>

            <div class="mb-2">
                <label>Phone</label>
                <input class="form-control" name="NewPlayers[${index}].Phone" />
            </div>
        </div>
    `;
}



document.addEventListener("DOMContentLoaded", function () {
	const filterBy = document.getElementById("filterBy");
	const divisionFilter = document.getElementById("divisionFilter");
	const paymentFilter = document.getElementById("paymentFilter");

	//Check for filter options to prevent errors.
	if (!filterBy || !divisionFilter || !paymentFilter) return;

	function updateFilterOptions() {
		if (filterBy.value === "Division") {
			divisionFilter.classList.remove("d-none");
			paymentFilter.classList.add("d-none");
		} else {
			paymentFilter.classList.remove("d-none");
			divisionFilter.classList.add("d-none");
		}
	}

	// Run on page load
	updateFilterOptions();

	// Run on selection change
	filterBy.addEventListener("change", updateFilterOptions);
});

function submitDelete(id) {
	document.getElementById("deleteForm-" + id).submit();
}

function getProvinceOptions(selected) {
	const provinces = ["AB", "BC", "MB", "NB", "NL", "NS", "ON", "PE", "QC", "SK", "NT", "NU", "YT"];
	return provinces
		.map(p => `<option value="${p}" ${p === selected ? "selected" : ""}>${p}</option>`)
		.join("");
}

function editPlayer(playerId, name, city, province, email, phone, teamId) {
    const container = document.getElementById("editPlayerContainer");

	container.innerHTML = `
        <form method="post" action="/Admin/UpdatePlayer">

            <input type="hidden" name="PlayerId" value="${playerId}">
            <input type="hidden" name="TeamId" value="${teamId}">

            <div class="row mb-2">
                <label class="col-4 col-form-label">Player Name:</label>
                <div class="col-8">
                    <input name="PlayerName" class="form-control" value="${name}">
                </div>
            </div>

            <div class="row mb-2">
                <label class="col-4 col-form-label">City:</label>
                <div class="col-8">
                    <input name="City" class="form-control" value="${city}">
                </div>
            </div>

            <div class="row mb-2">
                <label class="col-4 col-form-label">Province:</label>
                <div class="col-8">
                    <select name="Province" class="form-select">
                        ${getProvinceOptions(province)}
                    </select>
                </div>
            </div>

            <div class="row mb-2">
                <label class="col-4 col-form-label">Email:</label>
                <div class="col-8">
                    <input name="Email" class="form-control" value="${email}">
                </div>
            </div>

            <div class="row mb-2">
                <label class="col-4 col-form-label">Phone:</label>
                <div class="col-8">
                    <input name="Phone" class="form-control" value="${phone}">
                </div>
            </div>

            <button class="btn btn-success w-100 mt-2">Save Player Changes</button>
        </form>
    `;
}
