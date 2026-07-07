/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Views/**/*.cshtml",
    "./Areas/**/*.cshtml",
    "./wwwroot/js/**/*.js",
    "./wwwroot/js/*.js",
    "./*.cshtml"
  ],
  theme: {
    extend: {
      colors: {
        canvasia: {
          ink: "#22211f",
          forest: "#26413c",
          "forest-deep": "#1a2d29",
          gold: "#af8452",
          sand: "#f4efe7",
          mist: "#e6ddd0",
          cream: "#fbf8f2"
        },
        brand: {
          primary: '#1a2b1b',
          gold: '#b58735',
          cream: '#fcf9f3',
          olive: '#313511',
          lightgold: '#c6ca99',
          warmcream: '#f7f1e6',
          darktext: '#1c1c18',
          mutedtext: '#47473d',
          border: '#e5e2dc',
          darkborder: '#d8c9aa',
        }
      },
      fontFamily: {
        heading: ["Cairo", "sans-serif"],
        body: ["Cairo", "sans-serif"],
        sans: ['IBM Plex Sans Arabic', 'Inter', 'system-ui', 'sans-serif'],
      },
      boxShadow: {
        soft: "0 18px 45px rgba(34, 33, 31, 0.08)",
        panel: "0 24px 60px rgba(28, 43, 40, 0.1)"
      }
    }
  },
  plugins: []
};
