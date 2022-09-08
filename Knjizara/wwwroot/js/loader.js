    const loader = document.getElementById("loader");
    window.addEventListener("load", () => {
        loader.style.opacity = "0";
        loader.style.zIndex = "-1";
        loader.style.display = "none";
    });


	const msgContainer = document.getElementById("loaderMessages");

	const messages = [
		"Loading, please wait...",
		"Just a bit more...",
		"Almost there!"
	];

	let j = 0;

	msgContainer.innerText = messages[0];

	const changeMessage = () => {

		msgContainer.classList.add("show");

		msgContainer.innerHTML = messages[j];

		setTimeout(() => {msgContainer.classList.remove("show")}, 1800);
		
		if (j < messages.length - 1) {
			j++;
		}
		else {
			j = 0;
		}
	};

	setInterval(changeMessage, 2500);