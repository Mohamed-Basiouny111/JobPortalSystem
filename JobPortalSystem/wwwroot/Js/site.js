document.addEventListener("DOMContentLoaded", function () {
    const applyButtons = document.querySelectorAll('.apply-btn');

    applyButtons.forEach(btn => {
        btn.addEventListener('click', function () {
            const jobId = btn.dataset.jobid;
            window.location.href = `/JobApplication/Apply?jobId=${jobId}`;
        });
    });
});
