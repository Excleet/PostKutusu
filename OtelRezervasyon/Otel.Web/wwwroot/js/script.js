// Otel Rezervasyon Sistemi JavaScript

document.addEventListener('DOMContentLoaded', function() {
    // Navigation
    initNavigation();
    
    // Slider
    if (document.querySelector('.main-slider')) {
        initSlider();
    }
    
    // Forms
    initForms();
    
    // Smooth scrolling
    initSmoothScrolling();
});

// Navigation functions
function initNavigation() {
    const navbar = document.getElementById('navbar');
    const dropdownBtn = document.getElementById('dropdown-btn');
    const dropdownMenu = document.getElementById('dropdown-menu');
    const hamburger = document.getElementById('hamburger');
    const navMenu = document.getElementById('nav-menu');

    // Scroll effect
    window.addEventListener('scroll', function() {
        if (window.scrollY > 100) {
            navbar.classList.add('scrolled');
        } else {
            navbar.classList.remove('scrolled');
        }
    });

    // Desktop dropdown
    if (dropdownBtn && dropdownMenu) {
        dropdownBtn.addEventListener('click', function(e) {
            e.preventDefault();
            dropdownBtn.classList.toggle('active');
            dropdownMenu.classList.toggle('active');
        });

        // Close dropdown when clicking outside
        document.addEventListener('click', function(e) {
            if (!dropdownBtn.contains(e.target) && !dropdownMenu.contains(e.target)) {
                dropdownBtn.classList.remove('active');
                dropdownMenu.classList.remove('active');
            }
        });
    }

    // Mobile hamburger
    if (hamburger && navMenu) {
        hamburger.addEventListener('click', function() {
            hamburger.classList.toggle('active');
            navMenu.classList.toggle('active');
        });
    }

    // Close mobile menu when clicking on a link
    const navLinks = document.querySelectorAll('.nav-link');
    navLinks.forEach(link => {
        link.addEventListener('click', function() {
            if (hamburger && navMenu) {
                hamburger.classList.remove('active');
                navMenu.classList.remove('active');
            }
        });
    });
}

// Slider functions
function initSlider() {
    const slides = document.querySelectorAll('.slide');
    const indicators = document.querySelectorAll('.indicator');
    const prevBtn = document.querySelector('.prev-slide');
    const nextBtn = document.querySelector('.next-slide');
    const progressBar = document.querySelector('.progress-bar');
    
    let currentSlide = 0;
    let slideInterval;
    const slideTime = 5000; // 5 seconds

    function showSlide(index) {
        // Hide all slides
        slides.forEach(slide => slide.classList.remove('active'));
        indicators.forEach(indicator => indicator.classList.remove('active'));
        
        // Show current slide
        slides[index].classList.add('active');
        indicators[index].classList.add('active');
        
        // Update progress bar
        progressBar.style.width = '0%';
        setTimeout(() => {
            progressBar.style.width = '100%';
        }, 100);
    }

    function nextSlide() {
        currentSlide = (currentSlide + 1) % slides.length;
        showSlide(currentSlide);
    }

    function prevSlide() {
        currentSlide = (currentSlide - 1 + slides.length) % slides.length;
        showSlide(currentSlide);
    }

    function startSlideShow() {
        slideInterval = setInterval(nextSlide, slideTime);
    }

    function stopSlideShow() {
        clearInterval(slideInterval);
    }

    // Event listeners
    if (nextBtn) {
        nextBtn.addEventListener('click', function() {
            stopSlideShow();
            nextSlide();
            startSlideShow();
        });
    }

    if (prevBtn) {
        prevBtn.addEventListener('click', function() {
            stopSlideShow();
            prevSlide();
            startSlideShow();
        });
    }

    // Indicator clicks
    indicators.forEach((indicator, index) => {
        indicator.addEventListener('click', function() {
            stopSlideShow();
            currentSlide = index;
            showSlide(currentSlide);
            startSlideShow();
        });
    });

    // Pause on hover
    const sliderContainer = document.querySelector('.slider-container');
    if (sliderContainer) {
        sliderContainer.addEventListener('mouseenter', stopSlideShow);
        sliderContainer.addEventListener('mouseleave', startSlideShow);
    }

    // Initialize
    showSlide(0);
    startSlideShow();
}

// Form functions
function initForms() {
    // Date validation for reservation forms
    const checkinInputs = document.querySelectorAll('input[type="date"][name*="girisTarihi"], input[type="date"][name*="checkin"]');
    const checkoutInputs = document.querySelectorAll('input[type="date"][name*="cikisTarihi"], input[type="date"][name*="checkout"]');

    // Set minimum date to today
    const today = new Date().toISOString().split('T')[0];
    
    checkinInputs.forEach(input => {
        input.min = today;
        input.addEventListener('change', function() {
            // Update checkout minimum date
            const nextDay = new Date(this.value);
            nextDay.setDate(nextDay.getDate() + 1);
            const minCheckout = nextDay.toISOString().split('T')[0];
            
            checkoutInputs.forEach(checkoutInput => {
                checkoutInput.min = minCheckout;
                if (checkoutInput.value && checkoutInput.value <= this.value) {
                    checkoutInput.value = minCheckout;
                }
            });
        });
    });

    // Newsletter form
    const newsletterForm = document.querySelector('.newsletter-form');
    if (newsletterForm) {
        newsletterForm.addEventListener('submit', function(e) {
            e.preventDefault();
            const email = this.querySelector('input[type="email"]').value;
            if (email) {
                alert('E-bülten kaydınız alınmıştır. Teşekkür ederiz!');
                this.querySelector('input[type="email"]').value = '';
            }
        });
    }

    // Room slider
    initRoomSlider();
}

// Room slider
function initRoomSlider() {
    const roomsSlider = document.querySelector('.rooms-slider');
    const prevBtn = document.querySelector('.rooms-section .prev-btn');
    const nextBtn = document.querySelector('.rooms-section .next-btn');
    
    if (!roomsSlider) return;

    let currentIndex = 0;
    const roomItems = document.querySelectorAll('.room-item');
    const itemWidth = roomItems[0] ? roomItems[0].offsetWidth + 20 : 0; // 20px gap
    const visibleItems = Math.floor(roomsSlider.offsetWidth / itemWidth);
    const maxIndex = Math.max(0, roomItems.length - visibleItems);

    function updateSlider() {
        const translateX = -currentIndex * itemWidth;
        roomsSlider.style.transform = `translateX(${translateX}px)`;
        
        // Update button states
        if (prevBtn) prevBtn.disabled = currentIndex === 0;
        if (nextBtn) nextBtn.disabled = currentIndex >= maxIndex;
    }

    if (prevBtn) {
        prevBtn.addEventListener('click', function() {
            if (currentIndex > 0) {
                currentIndex--;
                updateSlider();
            }
        });
    }

    if (nextBtn) {
        nextBtn.addEventListener('click', function() {
            if (currentIndex < maxIndex) {
                currentIndex++;
                updateSlider();
            }
        });
    }

    // Initialize
    updateSlider();

    // Handle window resize
    window.addEventListener('resize', function() {
        const newVisibleItems = Math.floor(roomsSlider.offsetWidth / itemWidth);
        const newMaxIndex = Math.max(0, roomItems.length - newVisibleItems);
        if (currentIndex > newMaxIndex) {
            currentIndex = newMaxIndex;
        }
        updateSlider();
    });
}

// Smooth scrolling
function initSmoothScrolling() {
    const links = document.querySelectorAll('a[href^="#"]');
    
    links.forEach(link => {
        link.addEventListener('click', function(e) {
            const href = this.getAttribute('href');
            if (href === '#') return;
            
            const target = document.querySelector(href);
            if (target) {
                e.preventDefault();
                const offsetTop = target.offsetTop - 80; // Account for fixed navbar
                
                window.scrollTo({
                    top: offsetTop,
                    behavior: 'smooth'
                });
            }
        });
    });
}

// Gallery lightbox
function initGallery() {
    const galleryItems = document.querySelectorAll('.gallery-item img');
    const lightbox = document.getElementById('lightbox');
    const lightboxImg = document.getElementById('lightbox-img');
    const lightboxClose = document.getElementById('lightbox-close');

    if (!lightbox) return;

    galleryItems.forEach(img => {
        img.addEventListener('click', function() {
            lightboxImg.src = this.src;
            lightboxImg.alt = this.alt;
            lightbox.classList.add('active');
            document.body.style.overflow = 'hidden';
        });
    });

    function closeLightbox() {
        lightbox.classList.remove('active');
        document.body.style.overflow = '';
    }

    if (lightboxClose) {
        lightboxClose.addEventListener('click', closeLightbox);
    }

    lightbox.addEventListener('click', function(e) {
        if (e.target === this) {
            closeLightbox();
        }
    });

    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape' && lightbox.classList.contains('active')) {
            closeLightbox();
        }
    });
}

// Initialize gallery when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    initGallery();
});

// Utility functions
function formatCurrency(amount, currency = 'EUR') {
    return new Intl.NumberFormat('tr-TR', {
        style: 'currency',
        currency: currency
    }).format(amount);
}

function formatDate(date, locale = 'tr-TR') {
    return new Date(date).toLocaleDateString(locale, {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
}

// Export functions for external use
window.OtelJS = {
    formatCurrency,
    formatDate,
    initSlider,
    initForms,
    initNavigation
};
