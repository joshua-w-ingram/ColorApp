// Color Magic - A Fun Color App!
// This app helps you mix colors and learn about them!

// ============================================
// CRAYON COLORS - Fun named colors to explore!
// ============================================
const CRAYON_COLORS = [
    { name: "Red", r: 238, g: 32, b: 77 },
    { name: "Orange", r: 255, g: 117, b: 56 },
    { name: "Yellow", r: 252, g: 232, b: 131 },
    { name: "Green", r: 28, g: 172, b: 120 },
    { name: "Blue", r: 31, g: 117, b: 254 },
    { name: "Purple Heart", r: 116, g: 66, b: 200 },
    { name: "Hot Magenta", r: 255, g: 29, b: 206 },
    { name: "Electric Lime", r: 206, g: 255, b: 29 },
    { name: "Atomic Tangerine", r: 255, g: 164, b: 116 },
    { name: "Laser Lemon", r: 254, g: 254, b: 34 },
    { name: "Screamin' Green", r: 118, g: 255, b: 122 },
    { name: "Radical Red", r: 255, g: 73, b: 108 },
    { name: "Wild Watermelon", r: 252, g: 108, b: 133 },
    { name: "Outrageous Orange", r: 255, g: 110, b: 74 },
    { name: "Neon Carrot", r: 255, g: 163, b: 67 },
    { name: "Sunglow", r: 255, g: 207, b: 72 },
    { name: "Mango Tango", r: 255, g: 130, b: 67 },
    { name: "Pink Flamingo", r: 252, g: 116, b: 253 },
    { name: "Shocking Pink", r: 251, g: 126, b: 253 },
    { name: "Razzle Dazzle Rose", r: 255, g: 72, b: 208 },
    { name: "Purple Pizzazz", r: 254, g: 78, b: 218 },
    { name: "Aquamarine", r: 120, g: 219, b: 226 },
    { name: "Robin's Egg Blue", r: 31, g: 206, b: 203 },
    { name: "Caribbean Green", r: 28, g: 211, b: 162 },
    { name: "Turquoise Blue", r: 119, g: 221, b: 231 },
    { name: "Sky Blue", r: 128, g: 218, b: 235 },
    { name: "Cornflower", r: 154, g: 206, b: 235 },
    { name: "Cerulean", r: 29, g: 172, b: 214 },
    { name: "Pacific Blue", r: 28, g: 169, b: 201 },
    { name: "Blue Violet", r: 115, g: 102, b: 189 },
    { name: "Vivid Violet", r: 143, g: 80, b: 157 },
    { name: "Wisteria", r: 205, g: 164, b: 222 },
    { name: "Lavender", r: 252, g: 180, b: 213 },
    { name: "Carnation Pink", r: 255, g: 170, b: 204 },
    { name: "Cotton Candy", r: 255, g: 188, b: 217 },
    { name: "Tickle Me Pink", r: 252, g: 137, b: 172 },
    { name: "Magenta", r: 246, g: 100, b: 175 },
    { name: "Cerise", r: 221, g: 68, b: 146 },
    { name: "Wild Strawberry", r: 255, g: 67, b: 164 },
    { name: "Razzmatazz", r: 227, g: 37, b: 107 },
    { name: "Jazzberry Jam", r: 202, g: 55, b: 103 },
    { name: "Fuchsia", r: 195, g: 100, b: 197 },
    { name: "Orchid", r: 230, g: 168, b: 215 },
    { name: "Plum", r: 142, g: 69, b: 133 },
    { name: "Mulberry", r: 197, g: 75, b: 140 },
    { name: "Inchworm", r: 178, g: 236, b: 93 },
    { name: "Granny Smith Apple", r: 168, g: 228, b: 160 },
    { name: "Shamrock", r: 69, g: 206, b: 162 },
    { name: "Mountain Meadow", r: 48, g: 186, b: 143 },
    { name: "Jungle Green", r: 59, g: 176, b: 143 },
    { name: "Fern", r: 113, g: 188, b: 120 },
    { name: "Forest Green", r: 109, g: 174, b: 129 },
    { name: "Sea Green", r: 159, g: 226, b: 191 },
    { name: "Magic Mint", r: 170, g: 240, b: 209 },
    { name: "Teal Blue", r: 24, g: 167, b: 181 },
    { name: "Pine Green", r: 21, g: 128, b: 120 },
    { name: "Burnt Orange", r: 255, g: 127, b: 73 },
    { name: "Burnt Sienna", r: 234, g: 126, b: 93 },
    { name: "Copper", r: 221, g: 148, b: 117 },
    { name: "Tumbleweed", r: 222, g: 170, b: 136 },
    { name: "Tan", r: 250, g: 167, b: 108 },
    { name: "Peach", r: 255, g: 207, b: 171 },
    { name: "Melon", r: 253, g: 188, b: 180 },
    { name: "Macaroni and Cheese", r: 255, g: 189, b: 136 },
    { name: "Apricot", r: 253, g: 217, b: 181 },
    { name: "Banana Mania", r: 250, g: 231, b: 181 },
    { name: "Canary", r: 255, g: 255, b: 153 },
    { name: "Goldenrod", r: 252, g: 217, b: 117 },
    { name: "Dandelion", r: 253, g: 219, b: 109 },
    { name: "Gold", r: 231, g: 198, b: 151 },
    { name: "Maize", r: 237, g: 209, b: 156 },
    { name: "Almond", r: 239, g: 222, b: 205 },
    { name: "Desert Sand", r: 239, g: 205, b: 184 },
    { name: "Sepia", r: 165, g: 105, b: 79 },
    { name: "Brown", r: 180, g: 103, b: 77 },
    { name: "Raw Sienna", r: 214, g: 138, b: 89 },
    { name: "Mahogany", r: 205, g: 74, b: 76 },
    { name: "Chestnut", r: 188, g: 93, b: 88 },
    { name: "Brick Red", r: 203, g: 65, b: 84 },
    { name: "Maroon", r: 200, g: 56, b: 90 },
    { name: "Salmon", r: 255, g: 155, b: 170 },
    { name: "Scarlet", r: 252, g: 40, b: 71 },
    { name: "Sunset Orange", r: 253, g: 94, b: 83 },
    { name: "Bittersweet", r: 253, g: 124, b: 110 },
    { name: "Vivid Tangerine", r: 255, g: 160, b: 137 },
    { name: "Gray", r: 149, g: 145, b: 140 },
    { name: "Silver", r: 205, g: 197, b: 194 },
    { name: "Timberwolf", r: 219, g: 215, b: 210 },
    { name: "Black", r: 0, g: 0, b: 0 },
    { name: "White", r: 255, g: 255, b: 255 },
    { name: "Midnight Blue", r: 26, g: 72, b: 118 },
    { name: "Navy Blue", r: 25, g: 116, b: 210 },
    { name: "Denim", r: 43, g: 108, b: 196 },
    { name: "Indigo", r: 93, g: 118, b: 203 },
    { name: "Periwinkle", r: 197, g: 208, b: 230 },
    { name: "Outer Space", r: 65, g: 74, b: 76 },
    { name: "Manatee", r: 151, g: 154, b: 170 },
    { name: "Cadet Blue", r: 176, g: 183, b: 198 },
    { name: "Blue Bell", r: 162, g: 162, b: 208 },
    { name: "Wild Blue Yonder", r: 162, g: 173, b: 208 }
];

// Fun facts about colors!
const COLOR_FACTS = [
    "Red is the first color babies can see!",
    "Blue is the world's favorite color!",
    "Yellow and red together make you feel hungry - that's why fast food logos use them!",
    "Green is the easiest color for your eyes to see!",
    "Purple used to be so expensive that only kings and queens could wear it!",
    "Orange was named after the fruit, not the other way around!",
    "Pink used to be considered a boy's color 100 years ago!",
    "Cows can't see red - bulls charge at the movement, not the color!",
    "The human eye can see about 10 million different colors!",
    "There's no word for 'blue' in some languages!",
    "Mosquitoes love dark colors - wear white to avoid bites!",
    "Black isn't technically a color - it's the absence of light!",
    "White is all colors mixed together!",
    "Your favorite color might change as you grow up!",
    "Colors can affect your mood and how you feel!"
];

// ============================================
// COLOR CONVERSION FUNCTIONS
// These convert between different color formats
// ============================================

// Convert RGB to HEX (like #FF5733)
function rgbToHex(r, g, b) {
    const toHex = (n) => {
        const hex = Math.max(0, Math.min(255, Math.round(n))).toString(16);
        return hex.length === 1 ? '0' + hex : hex;
    };
    return '#' + toHex(r) + toHex(g) + toHex(b);
}

// Convert RGB to HSL (Hue, Saturation, Lightness)
function rgbToHsl(r, g, b) {
    r /= 255;
    g /= 255;
    b /= 255;

    const max = Math.max(r, g, b);
    const min = Math.min(r, g, b);
    let h, s, l = (max + min) / 2;

    if (max === min) {
        h = s = 0;
    } else {
        const d = max - min;
        s = l > 0.5 ? d / (2 - max - min) : d / (max + min);

        switch (max) {
            case r: h = ((g - b) / d + (g < b ? 6 : 0)) / 6; break;
            case g: h = ((b - r) / d + 2) / 6; break;
            case b: h = ((r - g) / d + 4) / 6; break;
        }
    }

    return {
        h: Math.round(h * 360),
        s: Math.round(s * 100),
        l: Math.round(l * 100)
    };
}

// Convert HSL to RGB
function hslToRgb(h, s, l) {
    h /= 360;
    s /= 100;
    l /= 100;

    let r, g, b;

    if (s === 0) {
        r = g = b = l;
    } else {
        const hue2rgb = (p, q, t) => {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;
            if (t < 1/6) return p + (q - p) * 6 * t;
            if (t < 1/2) return q;
            if (t < 2/3) return p + (q - p) * (2/3 - t) * 6;
            return p;
        };

        const q = l < 0.5 ? l * (1 + s) : l + s - l * s;
        const p = 2 * l - q;
        r = hue2rgb(p, q, h + 1/3);
        g = hue2rgb(p, q, h);
        b = hue2rgb(p, q, h - 1/3);
    }

    return {
        r: Math.round(r * 255),
        g: Math.round(g * 255),
        b: Math.round(b * 255)
    };
}

// Find the closest color name (for colorblind help!)
function findClosestColorName(r, g, b) {
    let closestColor = CRAYON_COLORS[0];
    let smallestDistance = Infinity;

    for (const color of CRAYON_COLORS) {
        // Calculate how different the colors are
        const distance = Math.sqrt(
            Math.pow(r - color.r, 2) +
            Math.pow(g - color.g, 2) +
            Math.pow(b - color.b, 2)
        );

        if (distance < smallestDistance) {
            smallestDistance = distance;
            closestColor = color;
        }
    }

    return closestColor.name;
}

// Convert RGB to CMYK (used for printing!)
function rgbToCmyk(r, g, b) {
    let c = 1 - (r / 255);
    let m = 1 - (g / 255);
    let y = 1 - (b / 255);
    let k = Math.min(c, m, y);

    if (k === 1) {
        return { c: 0, m: 0, y: 0, k: 100 };
    }

    c = ((c - k) / (1 - k)) * 100;
    m = ((m - k) / (1 - k)) * 100;
    y = ((y - k) / (1 - k)) * 100;
    k = k * 100;

    return {
        c: Math.round(c),
        m: Math.round(m),
        y: Math.round(y),
        k: Math.round(k)
    };
}

// ============================================
// APP STATE - Keeps track of the current color
// ============================================
let currentColor = { r: 128, g: 128, b: 128 };
let currentMode = 'rgb'; // 'rgb' or 'hsl'

// ============================================
// DOM ELEMENTS - Parts of the page we'll use
// ============================================
const colorBox = document.getElementById('color-box');
const saveBtn = document.getElementById('save-color-btn');
const randomBtn = document.getElementById('random-btn');

// RGB sliders
const rSlider = document.getElementById('r-slider');
const gSlider = document.getElementById('g-slider');
const bSlider = document.getElementById('b-slider');
const rValue = document.getElementById('r-value');
const gValue = document.getElementById('g-value');
const bValue = document.getElementById('b-value');

// HSL sliders
const hSlider = document.getElementById('h-slider');
const sSlider = document.getElementById('s-slider');
const lSlider = document.getElementById('l-slider');
const hValue = document.getElementById('h-value');
const sValue = document.getElementById('s-value');
const lValue = document.getElementById('l-value');

// Display elements
const hexDisplay = document.getElementById('hex-display');
const rgbDisplay = document.getElementById('rgb-display');
const hslDisplay = document.getElementById('hsl-display');
const cmykDisplay = document.getElementById('cmyk-display');
const factText = document.getElementById('fact-text');

// ============================================
// UPDATE FUNCTIONS - Keep everything in sync
// ============================================

// Update the color preview box and all displays
function updateColorDisplay() {
    const { r, g, b } = currentColor;
    const hex = rgbToHex(r, g, b);
    const hsl = rgbToHsl(r, g, b);
    const cmyk = rgbToCmyk(r, g, b);
    const colorName = findClosestColorName(r, g, b);

    // Update the color box
    colorBox.style.backgroundColor = hex;

    // Update the color name display (helps colorblind people!)
    document.getElementById('color-name-text').textContent = colorName;
    document.getElementById('color-hex-text').textContent = hex.toUpperCase();

    // Update info displays
    hexDisplay.textContent = hex.toUpperCase();
    rgbDisplay.textContent = `rgb(${r}, ${g}, ${b})`;
    hslDisplay.textContent = `hsl(${hsl.h}, ${hsl.s}%, ${hsl.l}%)`;
    cmykDisplay.textContent = `C:${cmyk.c}% M:${cmyk.m}% Y:${cmyk.y}% K:${cmyk.k}%`;

    // Update slider backgrounds for dynamic colors
    updateSliderBackgrounds();
}

// Update the slider track colors to show the current color range
function updateSliderBackgrounds() {
    const { r, g, b } = currentColor;

    // RGB sliders show what the color would look like at different values
    rSlider.style.background = `linear-gradient(to right, rgb(0,${g},${b}), rgb(255,${g},${b}))`;
    gSlider.style.background = `linear-gradient(to right, rgb(${r},0,${b}), rgb(${r},255,${b}))`;
    bSlider.style.background = `linear-gradient(to right, rgb(${r},${g},0), rgb(${r},${g},255))`;

    // HSL sliders
    const hsl = rgbToHsl(r, g, b);
    sSlider.style.background = `linear-gradient(to right, hsl(${hsl.h},0%,${hsl.l}%), hsl(${hsl.h},100%,${hsl.l}%))`;
    lSlider.style.background = `linear-gradient(to right, hsl(${hsl.h},${hsl.s}%,0%), hsl(${hsl.h},${hsl.s}%,50%), hsl(${hsl.h},${hsl.s}%,100%))`;
}

// Sync RGB sliders to current color
function syncRgbSliders() {
    rSlider.value = currentColor.r;
    gSlider.value = currentColor.g;
    bSlider.value = currentColor.b;
    rValue.textContent = currentColor.r;
    gValue.textContent = currentColor.g;
    bValue.textContent = currentColor.b;
}

// Sync HSL sliders to current color
function syncHslSliders() {
    const hsl = rgbToHsl(currentColor.r, currentColor.g, currentColor.b);
    hSlider.value = hsl.h;
    sSlider.value = hsl.s;
    lSlider.value = hsl.l;
    hValue.textContent = hsl.h;
    sValue.textContent = hsl.s;
    lValue.textContent = hsl.l;
}

// ============================================
// EVENT HANDLERS - When the user does stuff
// ============================================

// RGB slider changes
function handleRgbChange() {
    currentColor.r = parseInt(rSlider.value);
    currentColor.g = parseInt(gSlider.value);
    currentColor.b = parseInt(bSlider.value);
    rValue.textContent = currentColor.r;
    gValue.textContent = currentColor.g;
    bValue.textContent = currentColor.b;
    syncHslSliders();
    updateColorDisplay();
}

// HSL slider changes
function handleHslChange() {
    const h = parseInt(hSlider.value);
    const s = parseInt(sSlider.value);
    const l = parseInt(lSlider.value);
    hValue.textContent = h;
    sValue.textContent = s;
    lValue.textContent = l;

    const rgb = hslToRgb(h, s, l);
    currentColor = rgb;
    syncRgbSliders();
    updateColorDisplay();
}

// Random color
function randomColor() {
    currentColor = {
        r: Math.floor(Math.random() * 256),
        g: Math.floor(Math.random() * 256),
        b: Math.floor(Math.random() * 256)
    };
    syncRgbSliders();
    syncHslSliders();
    updateColorDisplay();
    showRandomFact();
}

// Show a random fun fact
function showRandomFact() {
    const fact = COLOR_FACTS[Math.floor(Math.random() * COLOR_FACTS.length)];
    factText.textContent = fact;
}

// ============================================
// TABS - Switch between different sections
// ============================================

function initTabs() {
    const tabs = document.querySelectorAll('.tab');
    const panels = document.querySelectorAll('.tab-panel');

    tabs.forEach(tab => {
        tab.addEventListener('click', () => {
            const target = tab.dataset.tab;

            // Update active tab
            tabs.forEach(t => t.classList.remove('active'));
            tab.classList.add('active');

            // Update active panel
            panels.forEach(p => p.classList.remove('active'));
            document.getElementById(target).classList.add('active');
        });
    });
}

// Mode toggle (RGB/HSL)
function initModeToggle() {
    const modeBtns = document.querySelectorAll('.mode-btn');
    const rgbSliders = document.getElementById('rgb-sliders');
    const hslSliders = document.getElementById('hsl-sliders');

    modeBtns.forEach(btn => {
        btn.addEventListener('click', () => {
            const mode = btn.dataset.mode;
            currentMode = mode;

            modeBtns.forEach(b => b.classList.remove('active'));
            btn.classList.add('active');

            if (mode === 'rgb') {
                rgbSliders.classList.remove('hidden');
                hslSliders.classList.add('hidden');
            } else {
                rgbSliders.classList.add('hidden');
                hslSliders.classList.remove('hidden');
            }
        });
    });
}

// ============================================
// SAVED COLORS - Save your favorite colors!
// ============================================

function getSavedColors() {
    const saved = localStorage.getItem('colorMagic_savedColors');
    return saved ? JSON.parse(saved) : [];
}

function saveColor() {
    const colors = getSavedColors();
    const hex = rgbToHex(currentColor.r, currentColor.g, currentColor.b);

    // Check if color already saved
    if (colors.some(c => c.hex === hex)) {
        showToast('This color is already saved!', 'error');
        return;
    }

    const colorName = prompt('Give your color a name:', hex);
    if (colorName === null) return; // Cancelled

    colors.push({
        hex: hex,
        name: colorName || hex,
        r: currentColor.r,
        g: currentColor.g,
        b: currentColor.b
    });

    localStorage.setItem('colorMagic_savedColors', JSON.stringify(colors));
    renderSavedColors();
    showToast('Color saved!', 'success');
}

function deleteColor(hex) {
    let colors = getSavedColors();
    colors = colors.filter(c => c.hex !== hex);
    localStorage.setItem('colorMagic_savedColors', JSON.stringify(colors));
    renderSavedColors();
    showToast('Color deleted!', 'success');
}

function loadColor(r, g, b) {
    currentColor = { r, g, b };
    syncRgbSliders();
    syncHslSliders();
    updateColorDisplay();

    // Switch to mixer tab
    document.querySelector('.tab[data-tab="mixer"]').click();
    showToast('Color loaded!', 'success');
}

function renderSavedColors() {
    const container = document.getElementById('saved-colors');
    const colors = getSavedColors();

    if (colors.length === 0) {
        container.innerHTML = '<p class="empty-message">No colors saved yet! Go mix some colors and save them!</p>';
        return;
    }

    container.innerHTML = colors.map(color => `
        <div class="color-swatch"
             style="background-color: ${color.hex}"
             onclick="loadColor(${color.r}, ${color.g}, ${color.b})">
            <span class="color-name">${color.name}</span>
            <button class="delete-btn" onclick="event.stopPropagation(); deleteColor('${color.hex}')">×</button>
        </div>
    `).join('');
}

// ============================================
// CRAYON COLORS - Pre-made colors to explore!
// ============================================

function renderCrayonColors(filter = '') {
    const container = document.getElementById('crayon-colors');
    const filtered = CRAYON_COLORS.filter(c =>
        c.name.toLowerCase().includes(filter.toLowerCase())
    );

    container.innerHTML = filtered.map(color => {
        const hex = rgbToHex(color.r, color.g, color.b);
        return `
            <div class="color-swatch"
                 style="background-color: ${hex}"
                 onclick="loadColor(${color.r}, ${color.g}, ${color.b})">
                <span class="color-name">${color.name}</span>
            </div>
        `;
    }).join('');
}

// ============================================
// COPY TO CLIPBOARD - Share color codes!
// ============================================

function initCopyButtons() {
    document.querySelectorAll('.copy-btn').forEach(btn => {
        btn.addEventListener('click', () => {
            const type = btn.dataset.copy;
            let text = '';

            switch(type) {
                case 'hex':
                    text = hexDisplay.textContent;
                    break;
                case 'rgb':
                    text = rgbDisplay.textContent;
                    break;
                case 'hsl':
                    text = hslDisplay.textContent;
                    break;
            }

            navigator.clipboard.writeText(text).then(() => {
                showToast('Copied: ' + text, 'success');
            }).catch(() => {
                showToast('Could not copy', 'error');
            });
        });
    });
}

// ============================================
// TOAST NOTIFICATIONS - Little popup messages
// ============================================

function showToast(message, type = 'success') {
    const toast = document.getElementById('toast');
    toast.textContent = message;
    toast.className = 'toast ' + type;

    setTimeout(() => {
        toast.classList.add('hidden');
    }, 2000);
}

// ============================================
// PHOTO COLOR PICKER - Olivia's Feature!
// Pick a color from any photo!
// ============================================

let photoCanvas = null;
let photoCtx = null;

function initPhotoPicker() {
    const photoInput = document.getElementById('photo-input');
    photoCanvas = document.getElementById('photo-canvas');
    photoCtx = photoCanvas.getContext('2d');

    // When a photo is selected
    photoInput.addEventListener('change', (e) => {
        const file = e.target.files[0];
        if (!file) return;

        const reader = new FileReader();
        reader.onload = (event) => {
            const img = new Image();
            img.onload = () => {
                // Size the canvas to fit the image
                const maxWidth = 450;
                const maxHeight = 400;
                let width = img.width;
                let height = img.height;

                // Scale down if too big
                if (width > maxWidth) {
                    height = (maxWidth / width) * height;
                    width = maxWidth;
                }
                if (height > maxHeight) {
                    width = (maxHeight / height) * width;
                    height = maxHeight;
                }

                photoCanvas.width = width;
                photoCanvas.height = height;
                photoCtx.drawImage(img, 0, 0, width, height);

                // Show the canvas, hide placeholder
                photoCanvas.classList.add('visible');
                document.getElementById('photo-placeholder').classList.add('hidden');

                showToast('Photo loaded! Tap anywhere to pick a color!', 'success');
            };
            img.src = event.target.result;
        };
        reader.readAsDataURL(file);
    });

    // When clicking on the photo
    photoCanvas.addEventListener('click', (e) => {
        const rect = photoCanvas.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;

        // Get the color at that spot
        const pixel = photoCtx.getImageData(x, y, 1, 1).data;
        const r = pixel[0];
        const g = pixel[1];
        const b = pixel[2];

        // Set this as the current color
        currentColor = { r, g, b };
        syncRgbSliders();
        syncHslSliders();
        updateColorDisplay();

        const colorName = findClosestColorName(r, g, b);
        showToast('Found: ' + colorName + '!', 'success');
    });
}

// ============================================
// INITIALIZE THE APP!
// ============================================

function init() {
    // Set up event listeners
    rSlider.addEventListener('input', handleRgbChange);
    gSlider.addEventListener('input', handleRgbChange);
    bSlider.addEventListener('input', handleRgbChange);

    hSlider.addEventListener('input', handleHslChange);
    sSlider.addEventListener('input', handleHslChange);
    lSlider.addEventListener('input', handleHslChange);

    randomBtn.addEventListener('click', randomColor);
    saveBtn.addEventListener('click', saveColor);

    // Search crayons
    document.getElementById('crayon-search').addEventListener('input', (e) => {
        renderCrayonColors(e.target.value);
    });

    // Initialize components
    initTabs();
    initModeToggle();
    initCopyButtons();
    initPhotoPicker();
    renderSavedColors();
    renderCrayonColors();

    // Set initial color display
    updateColorDisplay();
    showRandomFact();

    // Register service worker for offline support
    if ('serviceWorker' in navigator) {
        navigator.serviceWorker.register('sw.js')
            .then(() => console.log('Service Worker registered!'))
            .catch(err => console.log('Service Worker error:', err));
    }
}

// Start the app when the page loads
document.addEventListener('DOMContentLoaded', init);
